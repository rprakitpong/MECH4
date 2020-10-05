/*
 *  ======= exercise4 ========
 *
 *  Created on: Oct. 4, 2020
 *  Author:     Frienddo
 */

#include <msp430.h>
#include <exercise.h>

/*
 * ======== main ========
 */

// must use serial communicator exe that dr ma gave for this to work
// this code is based on sample code in class
int exercise4(void) {

    WDTCTL = WDTPW | WDTHOLD;   // stop watchdog timer

    // Configure clocks
    CSCTL0 = 0xA500;                        // Write password to modify CS registers
    CSCTL1 = DCOFSEL0 + DCOFSEL1;           // DCO = 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = DCO, ACLK = DCO, SMCLK = DCO

    // Configure LED out
    PJDIR |= PJ_ALLON;                        // Set to output
    PJOUT &= PJ_ALLOFF;                       // Set initial LED

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
    {
        __delay_cycles(1000000);
        unsigned char RxByte = 'a';
        while ((UCA0IFG & UCTXIFG)==0);
        UCA0TXBUF = RxByte;
    }

    return 0;
}

#pragma vector = USCI_A0_VECTOR
__interrupt void USCI_A0_ISR(void)
{
    unsigned char RxByte;
    RxByte = UCA0RXBUF;
    if (RxByte == 'j') {
        //led1 on
        PJOUT |= LED1;
    } else if (RxByte == 'k') {
        // led1 off
        PJOUT &= ~LED1;
    } else {
        // echo as usual
        while ((UCA0IFG & UCTXIFG)==0);
        UCA0TXBUF = RxByte;
        while ((UCA0IFG & UCTXIFG)==0);
        UCA0TXBUF = RxByte + 1;
    }
}


