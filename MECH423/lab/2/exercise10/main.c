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

    // Configure LED out
    PJDIR |= PJ_ALLON;                        // Set to output
    PJOUT &= PJ_ALLOFF;                       // Set initial LED

    // Set Timer B
    P3DIR |= BIT4 + BIT5;
    P3SEL1 &= ~(BIT4 + BIT5);
    P3SEL0 |= BIT4 + BIT5;

    // Set Timer B PWMs, one at 100% and one at 25%
    TB1CCR0 = 2000;
    TB1CCTL1 = OUTMOD_7;
    TB1CCR1 = 1000;
    TB1CCTL2 = OUTMOD_7;
    TB1CCR2 = 250;
    TB1CTL = TBSSEL_2 + MC_1 + TBCLR;

    while (1)
    {
        if (getBufCount() >= 5)
        {
            readByte = 0;
            while (!(readByte == 0xFF))
            { // find start byte and remove it
                readBuf();
            }
            char byte1 = 0;
            char byte2 = 0;
            char escByte = 0;
            if (readBuf())
            { // check command byte
                if (readByte == 0x01)
                {
                    // change wave
                    readBuf();
                    byte1 = readByte;
                    readBuf();
                    byte2 = readByte;
                    readBuf();
                    escByte = readByte;
                    int i = parseInt16(byte1, byte2, escByte);
                    yeet = i; // for checking in debugger
                    if (i > 2000 - 1) {
                        TB1CCR2 = 2000 - 1;
                    } else {
                        TB1CCR2 = i;
                    }
                }
                else if (readByte == 0x02)
                {
                    // LED1 on
                    PJOUT |= BIT0;
                    // flush rest of data
                    readBuf();
                    readBuf();
                    readBuf();
                }
                else if (readByte == 0x03)
                {
                    // LED1 off
                    PJOUT &= ~BIT0;
                    // flush rest of data
                    readBuf();
                    readBuf();
                    readBuf();
                }
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
