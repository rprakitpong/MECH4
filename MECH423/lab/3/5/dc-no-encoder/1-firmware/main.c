#include <msp430.h> 

/**
 * main.c
 */

#define LEN 51
#define TRUE 1
#define FALSE 0

#define PJ_ALLON (BIT0 + BIT1 + BIT2 + BIT3) // bits 0-3
#define PJ_ALLOFF 0

volatile unsigned int read = 0;
volatile unsigned char readByte = 0;
volatile unsigned int write = 0;
volatile unsigned char buffer[51] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0 };
volatile unsigned int yeet = 0;

volatile unsigned int timerCount = 0;

unsigned int plus1mod51(unsigned int i)
{
    // range: 0~51*2-1
    if (i + 1 < LEN)
    {
        return i + 1;
    }
    else
    {
        return (i + 1) - LEN;
    }
}

int readBuf()
{
    //read from buffer
    if (read == write)
    {
        return FALSE;
    }
    else
    {
        readByte = buffer[read];
        // doesn't delete old data from buffer, just wait for it to be overwritten later
        read = plus1mod51(read);
        return TRUE;
    }
}

int writeBuf(char n)
{
    if (read == plus1mod51(write))
    {
        // is full
        return FALSE;
    }
    else
    {
        buffer[write] = n;
        write = plus1mod51(write);
        return TRUE;
    }
}

int getBufCount()
{
    if (write >= read)
    {
        return write - read;
    }
    else
    {
        // read < write == write at beginning of array
        return (LEN - read) + write; // i think this is correct lol
    }
}

int parseInt16(char upperByte, char lowerByte, char escByte)
{
    if (escByte == 0x03)
    {
        upperByte = 0xFF;
        lowerByte = 0xFF;
    }
    else if (escByte == 0x02)
    {
        upperByte = 0xFF;
    }
    else if (escByte == 0x01)
    {
        lowerByte = 0xFF;
    }
    int ret = 0;
    ret |= (int) upperByte << 8;
    ret |= (int) lowerByte;
    return ret;
}

int main(void)
{
    WDTCTL = WDTPW | WDTHOLD;   // stop watchdog timer

    // Configure clocks
    CSCTL0 = 0xA500;                    // Write password to modify CS registers
    CSCTL1 = DCOFSEL0 + DCOFSEL1;           // DCO = 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = DCO, ACLK = DCO, SMCLK = DCO

    // Configure ports for UCA0
    P2SEL0 &= ~(BIT0 + BIT1);
    P2SEL1 |= BIT0 + BIT1;

    // Configure UCA0
    UCA0CTLW0 = UCSSEL0;
    UCA0BRW = 52;
    UCA0MCTLW = 0x4900 + UCOS16 + UCBRF0;
    UCA0IE |= UCRXIE;

    // global interrupt enable
    _EINT();

    P3DIR |= BIT5 + BIT6;
    P3OUT |= BIT5;
    P3OUT &= ~BIT6;

    // Set Timer B
    P3DIR |= BIT4;
    P3SEL1 &= ~(BIT4);
    P3SEL0 |= BIT4;

    // Set PWM out
    TB1CCR0 = 100;
    TB1CCTL1 = OUTMOD_7;
    TB1CCR1 = 0;
    //TB1CCTL2 = OUTMOD_7;
    //TB1CCR2 = 250;
    TB1CTL = TBSSEL_2 + MC_2 + TBCLR;

    // INTERRUPT
    TB2CTL |= TBSSEL_1 + MC_1;
    TB2CCR0 = 8000;     // arbitrary interval
    TB2CCTL0 = CCIE;    // enable interrupt

    int dcCount = 0;
    int dcCountUpTo = 0;
    int count = 0;
    int stopDC = FALSE;

    while (1)
    {
        if (timerCount > 1000 && stopDC == FALSE) {
            timerCount = 0;
            if (dcCount < dcCountUpTo) {
                dcCount = dcCount + 1;
            } else {
                TB1CCR1 = 0;
                stopDC = TRUE;
            }
        }

        if (getBufCount() >= 5)
        {
            readByte = 0;
            while (!(readByte == 0xFF))
            { // find start byte and remove it
                readBuf();
            }
            int x_stepper = 0; // 0~38, 19 == 0 displace
            int y_dc = 0;
            int speed = 0;
            char escByte = 0;

            readBuf();
            x_stepper = readByte;
            readBuf();
            y_dc = readByte;
            readBuf();
            speed = readByte;
            readBuf();
            escByte = readByte;

            if (speed == 0 || y_dc == 19)
            {
                TB1CCR1 = 0;
            }
            else
            {
                if (y_dc > 19)
                {
                    // set ccw
                    P3OUT |= BIT5;
                    P3OUT &= ~BIT6;
                    dcCountUpTo = (y_dc - 19);
                }
                else
                {
                    // set cw
                    P3OUT |= BIT6;
                    P3OUT &= ~BIT5;
                    dcCountUpTo = (19 - y_dc);
                }
                dcCount = 0;
                TB1CCR1 = speed;
                stopDC = FALSE;
                timerCount = 0;
            }
        }
    }

    return 0;
}

#pragma vector = USCI_A0_VECTOR
__interrupt void USCI_A0_ISR(void)
{
    unsigned char RxByte;
    RxByte = UCA0RXBUF;
    //write to buffer
    if (writeBuf(RxByte) == FALSE)
    {
        // send 0xFF 3 times as error msg if write fail
        while ((UCA0IFG & UCTXIFG) == 0)
            ;
        UCA0TXBUF = 0xFF;
        while ((UCA0IFG & UCTXIFG) == 0)
            ;
        UCA0TXBUF = 0xFF;
        while ((UCA0IFG & UCTXIFG) == 0)
            ;
        UCA0TXBUF = 0xFF;
    }
}

#pragma vector = TIMER2_B0_VECTOR
__interrupt void TIMER2_B0_ISR(void)
{
    timerCount++;
}
