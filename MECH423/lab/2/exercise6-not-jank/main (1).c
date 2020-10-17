#include <msp430.h> 

/**
 * main.c
 */
int main(void)
{
        WDTCTL = WDTPW | WDTHOLD;   // stop watchdog timer

        CSCTL0_H = 0xA5;

        // Set DCO to 8MHz
        CSCTL1 |= DCOFSEL_3;
        CSCTL2 = SELA_3 + SELS_3 + SELM_3;
        // Divide by 8 = DCO = 1 MHz
        CSCTL3 = DIVA_3 + DIVS_3 + DIVM_3;

        // configure P3.4 & P3.5 as output (these are connected to TB1.1 and TB1.2
        P3DIR |= BIT4 + BIT5;
        P3SEL1 &= ~(BIT4 + BIT5);
        P3SEL0 |= BIT4 + BIT5;

        // set Timer B in up count mode from SMCLK
        TB1CTL = TBSSEL_2 + MC_1 + TBCLR + TBIE;

        TB1CCR0 = 2000;
        TB1CCTL1 = OUTMOD_7;
        TB1CCTL2 = OUTMOD_7;
        TB1CCR1 = 1000;
        // Duty cycle of 7.5% so that the LED brightness is more discernable
        // TB1CCR2 would be 250 for the 25%
        TB1CCR2 = 75;

        // Configure ports for UCA0
           // Set to 10 for secondary module function
           P2SEL0 &= ~(BIT0 + BIT1);
           P2SEL1 |= BIT0 + BIT1;

           // Configure UCA0

           UCA0CTLW0 = UCSSEL0; // Use UCLK for UART

           // Use settings for 9600 baud - pg 490 of manual
           UCA0BRW = 52; // Divide by 52 for 9600 baud
           UCA0MCTLW = 0x4900 + UCOS16 + UCBRF0;



    return 0;
}

