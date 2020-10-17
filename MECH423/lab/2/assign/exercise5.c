/*
 *  ======= exercise5 ========
 *
 *  Created on: Oct. 5, 2020
 *  Author:     Frienddo
 */

#include <msp430.h>
#include <exercise.h>

/*
 * ======== main ========
 */
int exercise5(void)
{
    WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT
      CSCTL0_H = 0xA5;                          // unlock registers
      CSCTL2 |= SELS0 + SELS1;                  // Explicitly set SMCLK on DCO (should already be on DCO by default)
      CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set DCO =8MHz
      CSCTL3 |= DIVS0 + DIVS2;                  // Set SMCLK divider to 32 (should already be 32 by default)

      P1DIR |= BIT0 + BIT1;                     // Set P1.0 & P1.1 to out
      P3DIR |= LED5 + LED6;                     // Set LED5 & LED6 to out
      TB0CTL |= TASSEL_2 + MC_1 + TBIE;         // SMCLK, count mode, interrupt on
      TB0CCTL0 = CCIE;                          // TACCR0 interrupt enabled
      TB0CCR0 = 250;                            // input 8MHz/32 = 250kHz, output 1000Hz (25% duty), and 2 interrupts = 1 output period -> 250 input ticks before interrupting
      TB0CTL &= ~TBIFG;                         // Clear IFG

      __bis_SR_register(LPM4_bits + GIE);       // enable interrupts
      while(1) {
        __no_operation();
      }

      return 0;
    }
int counter = 0;

    #pragma vector=TIMER0_B0_VECTOR
    __interrupt void Timer_B_0(void)
    {
      // this interrupt happens at 1000Hz
      // to output 500Hz, toggle on at 0 and off at 2
      // to output 500Hz at 25% duty, toggle on at 0 and off at 1
      if (counter == 0) {
          P1OUT |= BIT0;
          P3OUT |= LED5;
          P1OUT |= BIT1;
          P3OUT |= LED6;
      }
      if (counter == 1) {
          P1OUT &= ~BIT1;
          P3OUT &= ~LED6;
      }
      if (counter == 2) {
          P1OUT &= ~BIT0;
          P3OUT &= ~LED5;
      }
      counter = counter + 1;
      if (counter == 4) {
          counter = 0;
      }

      TB0CTL &= ~TBIFG;                         // Clear IFG
    }

