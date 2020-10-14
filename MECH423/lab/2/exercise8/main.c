/*******************************************************************************
 *
 * main.c
 * User Experience Code for the MSP-EXP430FR5739
 * 
 *
 * Copyright (C) 2010 Texas Instruments Incorporated - http://www.ti.com/ 
 * 
 * 
 *  Redistribution and use in source and binary forms, with or without 
 *  modification, are permitted provided that the following conditions 
 *  are met:
 *
 *    Redistributions of source code must retain the above copyright 
 *    notice, this list of conditions and the following disclaimer.
 *
 *    Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the   
 *    distribution.
 *
 *    Neither the name of Texas Instruments Incorporated nor the names of
 *    its contributors may be used to endorse or promote products derived
 *    from this software without specific prior written permission.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
 *  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
 *  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 *  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
 *  OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 *  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 *  LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 *  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 *  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 *  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 *  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * Created: Version 1.0 04/13/2011
 *          Version 1.1 05/11/2011
 *          Version 1.2 08/31/2011
 *  
 ******************************************************************************/
#include "msp430.h"
#include "FR_EXP.h"

const unsigned char LED_Menu[] = {0x80,0xC0,0xE0,0xF0};
// These global variables are used in the ISRs and in FR_EXP.c
volatile unsigned char mode = 0;
volatile unsigned char UserInput = 0;
volatile unsigned char ULP =0; 
volatile unsigned int *FRAMPtr = 0;
volatile unsigned char active = 0;
volatile unsigned char SwitchCounter=0;
volatile unsigned char Switch1Pressed=0;
volatile unsigned char Switch2Pressed=0;
volatile unsigned int ADCResult = 0;

void main(void)
{  
  WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT  
  SystemInit();                             // Init the Board
  
  while(1)  
  {
    Mode4();
  }    
}
 
/**********************************************************************//**
 * @brief  ADC10 ISR for MODE3 and MODE4
 * 
 * @param  none 
 *  
 * @return none
 *************************************************************************/
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
             ADCResult = ADC10MEM0;
             __bic_SR_register_on_exit(CPUOFF);                                              
             break;                          // Clear CPUOFF bit from 0(SR)                         
    default: break; 
  }  
}



