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

void stepStepper(int p3, int p4, int p5, int p6)
{
    if (p3 == TRUE)
    {
        P1OUT |= BIT3;
    }
    else
    {
        P1OUT &= ~BIT3;
    }
    if (p4 == TRUE)
    {
        P1OUT |= BIT4;
    }
    else
    {
        P1OUT &= ~BIT4;
    }
    if (p5 == TRUE)
    {
        P1OUT |= BIT5;
    }
    else
    {
        P1OUT &= ~BIT5;
    }
    if (p6 == TRUE)
    {
        P1OUT |= BIT6;
    }
    else
    {
        P1OUT &= ~BIT6;
    }
}

unsigned int plus1loop8(state) {
    if (state == 7) {
        return 0;
    } else {
        return state + 1;
    }
}

unsigned int minus1loop8(state) {
    if (state == 0) {
        return 7;
    } else {
        return state - 1;
    }
}

int main(void)
{
    WDTCTL = WDTPW | WDTHOLD;   // stop watchdog timer

    // Configure clocks
    CSCTL0 = 0xA500;// Write password to modify CS registers
    CSCTL1 = DCOFSEL0 + DCOFSEL1;// DCO = 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1;// MCLK = DCO, ACLK = DCO, SMCLK = DCO

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

    // Stepper set up
    P1DIR |= BIT3 + BIT4 + BIT5 + BIT6;
    int speedCount = 0;
    int speedCountUpTo = 5000;
    int stepCount = 0;
    int stepCountUpTo = 0;
    unsigned int state = 0;
    int direction = TRUE;
    // ccw sequence
    int p3[8] =
    {   FALSE, FALSE, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE};
    int p4[8] =
    {   FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, FALSE, FALSE};
    int p5[8] =
    {   FALSE, TRUE, TRUE, TRUE, FALSE, FALSE, FALSE, FALSE};
    int p6[8] =
    {   TRUE, TRUE, FALSE, FALSE, FALSE, FALSE, FALSE, TRUE};
    stepStepper(p3[state], p4[state], p5[state], p6[state]);

    while (1)
    {
        yeet = speedCountUpTo;
        // do stepping
        if (speedCountUpTo != 0 && stepCount < stepCountUpTo) {
            if (speedCount >= speedCountUpTo)
            {
                if (direction == TRUE) // true = cw
                {
                    state = plus1loop8(state);
                }
                else
                {
                    state = minus1loop8(state);
                }
                stepStepper(p3[state], p4[state], p5[state], p6[state]);
                speedCount = 0;
                stepCount = stepCount + 1;
            }
            else
            {
                speedCount = speedCount + 1;
            }
        }

        // read uart buffer
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

            if (speed == 0 || x_stepper == 19)
            {
                // set cw
                direction = TRUE;
                speed = 0;
                stepCountUpTo = 0;
            }
            else
            {
                if (x_stepper > 19) {
                    // set cw
                    direction = TRUE;
                    stepCountUpTo = (x_stepper - 19)*64*16;
                } else {
                    // set ccw
                    direction = FALSE;
                    stepCountUpTo = (19 - x_stepper)*64*16;
                }
                speedCountUpTo = 718-speed*7;
            }
            speedCount = 0;
            stepCount = 0;
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
