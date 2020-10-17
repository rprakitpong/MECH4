#include <msp430.h>


/**
 * main.c
 */
volatile unsigned char Count, First_Time;
volatile unsigned int REdge1, REdge2, FEdge;

int main(void)
{
    WDTCTL = WDTPW + WDTHOLD;                               // Stop WDT
    CSCTL0_H = 0xA5;                                        // unlock registers
    CSCTL1 = DCOFSEL0 + DCOFSEL1;                           // Set DCO =8MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = ACLK = SMCLK = DCO

    // Set Timer B
    P3DIR |= BIT4 + BIT5;
    P3SEL1 &= ~(BIT4 + BIT5);
    P3SEL0 |= BIT4 + BIT5;

    // Set Timer B PWMs, one at 100% and one at 25%
    TB1CCR0 = 2000 - 1;
    TB1CCTL1 = OUTMOD_7;
    TB1CCR1 = 1000;
    TB1CCTL2 = OUTMOD_7;
    TB1CCR2 = 250;
    TB1CTL = TBSSEL_2 + MC_1 + TBCLR;

    // Set Timer A
    P1DIR &= ~BIT2;
    P1SEL1 &= ~BIT2;
    P1SEL0 |= BIT2;

    // Set Timer A for capture
    TA1CCTL1 = CAP + CM_3 + CCIE + SCS + CCIS_0;
    TA1CTL |= TASSEL_2 + MC_2 + TACLR;

    Count = 0x0;
    First_Time = 0x01;

    while(1)
    {
        __bis_SR_register(LPM0_bits + GIE);
        __no_operation();
        // On exiting LPM0
        if (TA0CCTL1 & COV)                     // Check for overflow
          while(1);                             // Loop
    }

    return 0;
}
#pragma vector = TIMER1_A1_VECTOR
__interrupt void TIMER1_A1_ISR(void)
{
  switch(__even_in_range(TA1IV,0x0A))
  {
    case  TA1IV_NONE: break;              // Vector  0:  No interrupt
    case  TA1IV_TACCR1:                   // Vector  2:  TACCR1 CCIFG
      if (TA1CCTL1 & CCI)                 // Capture Input Pin Status
      {
        // Rising Edge was captured
        if (!Count)
        {
          REdge1 = TA1CCR1;
          Count++;
        }
        else
        {
          REdge2 = TA1CCR1;
          Count=0x0;
          __bic_SR_register_on_exit(LPM0_bits + GIE);  // Exit LPM0 on return to main
        }

        if (First_Time)
          First_Time = 0x0;
        }
      else
      {
        // Falling Edge was captured
        if(!First_Time)
        {
          FEdge = TA1CCR1;
        }
      }
      break;
    case TA1IV_TACCR2: break;             // Vector  4:  TACCR2 CCIFG
    case TA1IV_6: break;                  // Vector  6:  Reserved CCIFG
    case TA1IV_TAIFG: break;              // Vector 10:  TAIFG
    default:  break;
  }
}
