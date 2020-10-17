/*******************************************************************************
 *
 * FR_EXP.c
 * User Experience Code for the MSP-EXP430FR5739
 * C Functions File
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
#include "msp430fr5739.h"
#include "FR_EXP.h"

extern volatile unsigned int ADCResult;

volatile unsigned char TX_Buffer[7] = {0,0,0,0,0,0,0};
volatile unsigned char ThreshRange[3]={0,0,0};
unsigned char counter =0;
unsigned int WriteCounter = 0;
unsigned int LEDCounter=0;
unsigned char ULPBreakSync =0;
unsigned int ModeAddress=0;
unsigned int CalValue =0;
unsigned int ADCTemp =0;
unsigned char temp = 0;


/**********************************************************************//**
 * @brief  Initializes system 
 * 
 * @param  none 
 *  
 * @return none
 *************************************************************************/
void Setup(void)
{
  // Startup clock system in max. DCO setting ~8MHz
  // This value is closer to 10MHz on untrimmed parts
  CSCTL0_H = 0xA5;                          // Unlock register
  CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set max. DCO setting
  CSCTL2 = SELA_1 + SELS_3 + SELM_3;        // set ACLK = vlo; MCLK = DCO
  CSCTL3 = DIVA_0 + DIVS_0 + DIVM_0;        // set all dividers
  CSCTL0_H = 0x01;                          // Lock Register
  
  // Enable LEDs
  P3OUT &= ~(BIT6+BIT7+BIT5+BIT4);
  P3DIR |= BIT6+BIT7+BIT5+BIT4;
  PJOUT &= ~(BIT0+BIT1+BIT2+BIT3);
  PJDIR |= BIT0 +BIT1+BIT2+BIT3;
  
  // Configure UART pins P2.0 & P2.1
  P2SEL1 |= BIT0 + BIT1;
  P2SEL0 &= ~(BIT0 + BIT1);
  
  // Configure UART 0
  UCA0CTL1 |= UCSWRST;
  UCA0CTL1 = UCSSEL_2;                      // Set SMCLK as UCLk
  UCA0BR0 = 52 ;                              // 9600 baud
  // 8000000/(9600*16) - INT(8000000/(9600*16))=0.083
  UCA0BR1 = 0;
  // UCBRFx = 1, UCBRSx = 0x49, UCOS16 = 1 (Refer User Guide)
  UCA0MCTLW = 0x4911 ;
  UCA0CTL1 &= ~UCSWRST;                     // release from reset

  // P3.0,P3.1 and P3.2 are accelerometer inputs
  P3OUT &= ~(BIT0 + BIT1 + BIT2);   
  P3DIR &= ~(BIT0 + BIT1 + BIT2); 
  P3REN |= BIT0 + BIT1 + BIT2;

  // configure Timer A
  TA0CTL |= TASSEL_2 + MC_1 + TAIE;         // SMCLK, count mode, interrupt on
  TA0CCTL0 = CCIE;                          // TACCR0 interrupt enabled
  TA0CCR0 = 320000;
  TA0CTL &= ~TAIFG;                         // Clear IFG
}

void MainLoop(void)
{ 
  // One time initialization of header and footer transmit package
  TX_Buffer[0] = 0xFA;    
  TX_Buffer[6] = 0xFE;  
        
  // variable initialization
  ADCTemp = 0;
  temp = 0;
  WriteCounter = 0;   
  ULPBreakSync = 0;
  counter = 0;
  
  // One time setup and calibration
  SetupAccel();
  while (1)
    {  
      // Take 1 ADC Sample
      while (ADC10CTL1 & BUSY);
        ADC10CTL0 |= ADC10ENC | ADC10SC ;       // Start conversion
        __bis_SR_register(CPUOFF + GIE);        // LPM0, ADC10_ISR will force exit
        __no_operation();                       // For debug only
     }
}

void SetupAccel(void)
{  
  //Setup  accelerometer
  // ~20KHz sampling
  //Configure GPIO
  ACC_PORT_SEL0 |= ACC_X_PIN + ACC_Y_PIN + ACC_Z_PIN;    //Enable A/D channel inputs
  ACC_PORT_SEL1 |= ACC_X_PIN + ACC_Y_PIN + ACC_Z_PIN;
  ACC_PORT_DIR &= ~(ACC_X_PIN + ACC_Y_PIN + ACC_Z_PIN);
  ACC_PWR_PORT_DIR |= ACC_PWR_PIN;              //Enable ACC_POWER
  ACC_PWR_PORT_OUT |= ACC_PWR_PIN;

  // Allow the accelerometer to settle before sampling any data 
  __delay_cycles(200000);   
  
  //Single channel, once, 
  ADC10CTL0 &= ~ADC10ENC;                        // Ensure ENC is clear
  ADC10CTL0 = ADC10ON + ADC10SHT_5 + ADC10MSC;
  ADC10CTL1 = ADC10SHS_0 + ADC10SHP + ADC10CONSEQ_1 + ADC10SSEL_0;
  ADC10CTL2 = ADC10RES;    
  ADC10MCTL0 = ADC10SREF_0 + ADC10INCH_12;
  ADC10IV = 0x00;                          // Clear all ADC12 channel int flags  
  ADC10IE |= ADC10IE0;
}
