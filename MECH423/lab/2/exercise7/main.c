#include "msp430.h"

unsigned char ADC_Result[3];                // 8-bit ADC conversion result array

int main(void)
{
  WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT

  CSCTL0 = 0xA500;                          // Write password to modify CS registers
  CSCTL1 = DCOFSEL0 + DCOFSEL1;             // DCO = 8 MHz
  CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = DCO, ACLK = DCO, SMCLK = DCO

  // Configure UART pins P2.0 & P2.1
  P2SEL1 |= BIT0 + BIT1;
  P2SEL0 &= ~(BIT0 + BIT1);

  // Configure UCA0
  UCA0CTLW0 = UCSSEL0;
  UCA0BRW = 52;
  UCA0MCTLW = 0x4900 + UCOS16 + UCBRF0;

  P2OUT |= BIT7;
  P2DIR |= BIT7;

  P3DIR &= ~(BIT0 + BIT1 + BIT2);
  P3SEL0 |= BIT0 + BIT1 + BIT2;
  P3SEL1 |= BIT0 + BIT1 + BIT2;
  P3OUT &= ~(BIT0 + BIT1 + BIT2);
  P3REN |= BIT0 + BIT1 + BIT2;

  ADC10CTL0 &= ~ADC10ENC;
  // Configure ADC pins
  // Configure ADC10
  ADC10CTL0 = ADC10SHT_2 + ADC10MSC + ADC10ON;  // 16ADCclks, MSC, ADC ON
  ADC10CTL1 = ADC10SHP + ADC10CONSEQ_1;         // sampling timer, s/w trig.,single sequence
  ADC10CTL2 &= ~ADC10RES;                       // 8-bit resolution
  ADC10MCTL0 = ADC10INCH_14;                    // A12,A13,A14, AVCC reference

  // Configure DMA0 (ADC10IFG trigger)
  DMACTL0 = DMA0TSEL__ADC10IFG;                 // ADC10IFG trigger
  __data16_write_addr((unsigned short) &DMA0SA,(unsigned long) &ADC10MEM0);
                                                // Source single address
  __data16_write_addr((unsigned short) &DMA0DA,(unsigned long) &ADC_Result[0]);
                                                // Destination array address
  DMA0SZ = 0x03;                                // 3 conversions
  DMA0CTL = DMADT_4 + DMADSTINCR_3 + DMASRCBYTE + DMADSTBYTE + DMAEN + DMAIE;
                                                // Rpt, inc dest, byte access,
                                                // enable int after seq of convs

  // configure Timer A
  TA0CTL |= TASSEL_2 + MC_1 + TAIE;         // SMCLK, count mode, interrupt on
  TA0CCTL0 = CCIE;                          // TACCR0 interrupt enabled
  TA0CCR0 = 320000;                         // sample at 25Hz, smclk at dco 8MHz -> 8M/25 = sample every 320000 ticks
  TA0CTL &= ~TAIFG;                         // Clear IFG

  while(1)
  {
    while (ADC10CTL1 & BUSY);               // Wait if ADC10 core is active
    ADC10CTL0 |= ADC10ENC + ADC10SC;        // Sampling and conversion start
    __bis_SR_register(LPM0_bits + GIE);     // LPM0, ADC10_ISR will force exit
    __no_operation();                       // BREAKPOINT; check ADC_Result
  }

}

#pragma vector=DMA_VECTOR
__interrupt void DMA0_ISR (void)
{
  switch(__even_in_range(DMAIV,16))
  {
    case  0: break;                          // No interrupt
    case  2:
      // sequence of conversions complete
      __bic_SR_register_on_exit(CPUOFF);     // exit LPM
      break;                                 // DMA0IFG
    case  4: break;                          // DMA1IFG
    case  6: break;                          // DMA2IFG
    case  8: break;                          // Reserved
    case 10: break;                          // Reserved
    case 12: break;                          // Reserved
    case 14: break;                          // Reserved
    case 16: break;                          // Reserved
    default: break;
  }
}


#pragma vector=TIMER0_A0_VECTOR
__interrupt void Timer_A(void)
{
  while (!(UCA0IFG&UCTXIFG));                // USCI_A0 TX buffer ready?
  UCA0TXBUF = 0xFF;
  unsigned char counter = 0;
  while(counter<3)
  {
    while (!(UCA0IFG&UCTXIFG));              // USCI_A0 TX buffer ready?
    UCA0TXBUF = ADC_Result[counter];
    counter++;
  }

  TA0CTL &= ~TAIFG;                          // Clear IFG
}

