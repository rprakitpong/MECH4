
#include "msp430.h"
#include "FR_EXP.h"

const unsigned char LED_Menu[] = {0x80,0xC0,0xE0,0xF0};
// These global variables are used in the ISRs and in FR_EXP.c
volatile unsigned char active = 0;
volatile unsigned char SwitchCounter=0;
volatile unsigned char Switch1Pressed=0;
volatile unsigned char Switch2Pressed=0;
volatile unsigned int ADCResult_x = 0;
volatile unsigned int ADCResult_y = 0;
volatile unsigned int ADCResult_z = 0;
volatile unsigned int state = 1; // 1 for 12, 2 for 13, 3 for 14


void main(void)
{  
  WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT  
  Setup();                             // Init the Board

  MainLoop();
}
 
#pragma vector=ADC10_VECTOR
__interrupt void ADC10_ISR(void)
{
  switch(__even_in_range(ADC10IV,ADC10IV_ADC10IFG))
  {
    case ADC10IV_NONE: break;               // No interrupt
    case ADC10IV_ADC10OVIFG: break;         // conversion result overflow
    case ADC10IV_ADC10TOVIFG: break;        // conversion time overflow
    case ADC10IV_ADC10HIIFG: break;         // ADC10HI
    case ADC10IV_ADC10LOIFG: break;         // ADC10LO
    case ADC10IV_ADC10INIFG: break;         // ADC10IN
    case ADC10IV_ADC10IFG:
        if (state == 1) {
            ADCResult_x = ADC10MEM0;
            ADC10MCTL0 = ADC10SREF_0 + ADC10INCH_13;
            state = 2;
        } else if (state == 2) {
            ADCResult_y = ADC10MEM0;
            ADC10MCTL0 = ADC10SREF_0 + ADC10INCH_14;
            state = 3;
        } else if (state == 3) {
            ADCResult_z = ADC10MEM0;
            ADC10MCTL0 = ADC10SREF_0 + ADC10INCH_12;
            state = 1;
        }
             __bic_SR_register_on_exit(CPUOFF);                                              
             break;                          // Clear CPUOFF bit from 0(SR)                         
    default: break; 
  }  
}

#pragma vector=TIMER0_A0_VECTOR
__interrupt void Timer_A(void)
{
  while (!(UCA0IFG&UCTXIFG));                // USCI_A0 TX buffer ready?
  UCA0TXBUF = 0xFF;
  while (!(UCA0IFG&UCTXIFG));                // USCI_A0 TX buffer ready?
  UCA0TXBUF = ADCResult_x;
  while (!(UCA0IFG&UCTXIFG));                // USCI_A0 TX buffer ready?
  UCA0TXBUF = ADCResult_y;
  while (!(UCA0IFG&UCTXIFG));                // USCI_A0 TX buffer ready?
  UCA0TXBUF = ADCResult_z;

  TA0CTL &= ~TAIFG;                          // Clear IFG
}


