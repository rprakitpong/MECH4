#include <msp430.h> 

/**
 * main.c
 */

#define LEN 51

volatile unsigned int read = 0;
volatile unsigned int write = 0;
volatile unsigned char buffer[51] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0 };

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

    while (1)
        ;

    return 0;
}

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

#pragma vector = USCI_A0_VECTOR
__interrupt void USCI_A0_ISR(void)
{
    unsigned char RxByte;
    RxByte = UCA0RXBUF;
    if (RxByte == 13)
    {
        //read
        if (read == write)
        {
            // empty
            // send 0xFF 3 times as error msg
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
        else
        {
            while ((UCA0IFG & UCTXIFG) == 0)
                ;
            UCA0TXBUF = buffer[read];
            // doesn't delete old data from buffer, just wait for it to be overwritten later
            read = plus1mod51(read);
        }

    }
    else
    {
        //write
        if (read == plus1mod51(write))
        {
            // is full
            // send 0xFF 3 times as error msg
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
        else
        {
            buffer[write] = RxByte;
            write = plus1mod51(write);
        }
    }
}
