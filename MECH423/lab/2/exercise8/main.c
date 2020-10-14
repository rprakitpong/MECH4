#include "msp430.h"
#include "FR_EXP.h"

volatile unsigned int ADCResult = 0;

void main(void)
{
    WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT
    SystemInit();                             // Init the Board

    Mode4();
}

// referenced some sample code
#pragma vector=ADC10_VECTOR
__interrupt void ADC10_ISR(void)
{
    switch (__even_in_range(ADC10IV, ADC10IV_ADC10IFG))
    {
    case ADC10IV_ADC10IFG:
        ADCResult = ADC10MEM0;
        __bic_SR_register_on_exit(CPUOFF);
        break;           // Clear CPUOFF bit from 0(SR)
    default:
        break;
    }
}

