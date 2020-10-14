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

unsigned int WriteCounter = 0;
unsigned int CalValue = 0;
unsigned int ADCTemp = 0;
unsigned char temp = 0;

/**********************************************************************//**
 * @brief  Initializes system 
 * 
 * @param  none 
 *  
 * @return none
 *************************************************************************/
void SystemInit(void)
{
    // Startup clock system in max. DCO setting ~8MHz
    // This value is closer to 10MHz on untrimmed parts
    CSCTL0_H = 0xA5;                          // Unlock register
    CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set max. DCO setting
    CSCTL2 = SELA_1 + SELS_3 + SELM_3;        // set ACLK = vlo; MCLK = DCO
    CSCTL3 = DIVA_0 + DIVS_0 + DIVM_0;        // set all dividers
    CSCTL0_H = 0x01;                          // Lock Register

    // Turn off temp.
    REFCTL0 |= REFTCOFF;
    REFCTL0 &= ~REFON;

    // Enable switches
    // P4.0 and P4.1 are configured as switches
    // Port 4 has only two pins
    /*
     P4OUT |= BIT0 +BIT1;                      // Configure pullup resistor
     P4DIR &= ~(BIT0 + BIT1);                  // Direction = input
     P4REN |= BIT0 + BIT1;                     // Enable pullup resistor
     P4IES &= ~(BIT0+BIT1);                    // P4.0 Lo/Hi edge interrupt
     P4IE = BIT0+BIT1;                         // P4.0 interrupt enabled
     P4IFG = 0;                                // P4 IFG cleared
     */

    // Enable LEDs
    P3OUT &= ~(BIT6 + BIT7 + BIT5 + BIT4);
    P3DIR |= BIT6 + BIT7 + BIT5 + BIT4;
    PJOUT &= ~(BIT0 + BIT1 + BIT2 + BIT3);
    PJDIR |= BIT0 + BIT1 + BIT2 + BIT3;
    // Turn on LED1
    PJOUT |= BIT0;

    // Configure UART pins P2.0 & P2.1
    P2SEL1 |= BIT0 + BIT1;
    P2SEL0 &= ~(BIT0 + BIT1);
    // Configure UART 0
    UCA0CTL1 |= UCSWRST;
    UCA0CTL1 = UCSSEL_2;                      // Set SMCLK as UCLk
    UCA0BR0 = 52;                              // 9600 baud
    // 8000000/(9600*16) - INT(8000000/(9600*16))=0.083
    UCA0BR1 = 0;
    // UCBRFx = 1, UCBRSx = 0x49, UCOS16 = 1 (Refer User Guide)
    UCA0MCTLW = 0x4911;
    UCA0CTL1 &= ~UCSWRST;                     // release from reset

    // P2.7 is used to power the voltage divider for the NTC thermistor
    P2OUT &= ~BIT7;
    P2DIR |= BIT7;
}

void Mode4(void)
{
    // variable initialization
    ADCTemp = 0;
    temp = 0;
    WriteCounter = 0;

    // One time setup and calibration
    SetupThermistor();
    CalValue = CalibrateADC();
    while (1)
    {
        __delay_cycles(500000);
        // Take 1 ADC Sample
        TakeADCMeas(); // result in ADCResult

        if (ADCResult >= CalValue)
        {
            ADCTemp = ADCResult - CalValue;
            LEDSequence(ADCTemp);
        }

        WriteCounter++;
        if (WriteCounter > 0)
        {
            // Every 300 samples
            // Transmit 7 Bytes
            // Prepare mode-specific data
            // Standard header and footer
            WriteCounter = 0;
            TXData();
        }
    }
}

/**********************************************************************//**
 * @brief  Calibrate thermistor or accelerometer
 * 
 * @param  none 
 *  
 * @return none
 *************************************************************************/
unsigned int CalibrateADC(void)
{
    unsigned char CalibCounter = 0;
    unsigned int Value = 0;

    while (CalibCounter < 50)
    {
        P3OUT ^= BIT4;
        CalibCounter++;
        while (ADC10CTL1 & BUSY)
            ;
        ADC10CTL0 |= ADC10ENC | ADC10SC;       // Start conversion
        __bis_SR_register(CPUOFF + GIE);      // LPM0, ADC10_ISR will force exit
        __no_operation();
        Value += ADCResult;
    }
    Value = Value / 50;
    return Value;
}

/**********************************************************************//**
 * @brief  Take ADC Measurement
 * 
 * @param  none 
 *  
 * @return none
 *************************************************************************/
void TakeADCMeas(void)
{
    while (ADC10CTL1 & BUSY)
        ;
    ADC10CTL0 |= ADC10ENC | ADC10SC;       // Start conversion
    __bis_SR_register(CPUOFF + GIE);        // LPM0, ADC10_ISR will force exit
    __no_operation();                       // For debug only
}

/**********************************************************************//**
 * @brief  Setup thermistor
 * 
 * @param  none 
 *  
 * @return none
 *************************************************************************/
void SetupThermistor(void)
{
// ~16KHz sampling
//Turn on Power
    P2DIR |= BIT7;
    P2OUT |= BIT7;

// Configure ADC
    P1SEL1 |= BIT4;
    P1SEL0 |= BIT4;

// Allow for settling delay
    __delay_cycles(50000);

// Configure ADC
    ADC10CTL0 &= ~ADC10ENC;
    ADC10CTL0 = ADC10SHT_7 + ADC10ON;        // ADC10ON, S&H=192 ADC clks
// ADCCLK = MODOSC = 5MHz
    ADC10CTL1 = ADC10SHS_0 + ADC10SHP + ADC10SSEL_0;
    ADC10CTL2 = ADC10RES;                    // 10-bit conversion results
    ADC10MCTL0 = ADC10INCH_4;                // A4 ADC input select; Vref=AVCC
    ADC10IE = ADC10IE0;                    // Enable ADC conv complete interrupt
}

/**********************************************************************//**
 * @brief  LED Toggle Sequence
 * 
 * @param  
 * DiffValue Difference between calibrated and current measurement
 * temp Direction of difference (positive or negative)
 * @return none
 *************************************************************************/
int currLED = 0;
int init = 15;
int inc = 2; // 4*6+15=24+15=39
int LED[8] = {BIT0, BIT1, BIT2, BIT3, BIT4, BIT5, BIT6, BIT7 };

void LEDSequence(unsigned int DiffValue)
{
// thres 15, 25, 45, so with 7LED, use 6 inc

    //P3OUT &= ~(BIT6 + BIT7 + BIT5 + BIT4);
    //PJOUT &= ~(BIT0 + BIT1 + BIT2 + BIT3);
    //P3OUT |= LED[4];
    int temp = currLED;

    if ((DiffValue > currLED * inc + init) && (currLED < 7))
    {
        temp = 1 + currLED;
        if (temp > 3)
        {
            P3OUT |= LED[temp];
        }
        else
        {
            PJOUT |= LED[temp];
        }
    }
    else
    {
        if ((DiffValue < (currLED * inc) - inc + init) && (currLED > 0))
        {
            temp = currLED - 1;
            if (currLED > 3)
            {
                P3OUT &= ~LED[currLED];
            }
            else
            {
                PJOUT &= ~LED[currLED];
            }
        }
    }

    currLED = temp;
}

/**********************************************************************//**
 * @brief  Transmit 7 bytes
 * 
 * @param  none 
 *  
 * @return none
 *************************************************************************/
void TXData()
{
    while (!(UCA0IFG & UCTXIFG))
        ;             // USCI_A0 TX buffer ready?
    UCA0TXBUF = ADCResult;
}
