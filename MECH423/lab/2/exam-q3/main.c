
/*
 * 3-Axis Shock Sensor
Set up the ADC to periodically sample data from all three axes of the accelerometer (Ports A13, A14, A15) and shift the 10-bit results to 8-bit values.
Select a threshold acceleration value for each axis. If acceleration on each axis exceed the threshold acceleration (i.e. shock detected), turn on the LED on PJ.0, PJ.1, and PJ.2 for X, Y, and Z axes respectively.
Hold the LED on for ~2 s and then turn it off.
Each time the LED turns on (i.e. for each axis that detected a shock event), transmit a single packet to report the threshold acceleration and measured acceleration for that axis. No other packets should be transmitted while the LED is still on. Use the following packet format:

Start byte

Axis byte

Threshold data byte

Acceleration data byte

0xFF

0x01, 0x02, or 0x03

0x00 to 0xFE

0x00 to 0xFE

Place the MSP430EXP board on a table, tap it to create accelerations in the X, Y, and Z axes. Show the correct LED is lit for each axis.
Use the MSP430 Serial Communicator to check the transmitted message is reasonable and the acceleration data byte is greater than your defined threshold.
Record a video and upload to Canvas.
 */

#include <msp430.h>

volatile unsigned int xResult;
volatile unsigned int yResult;
volatile unsigned int zResult;
volatile unsigned int state = 1;

unsigned int xThres = 128 + 30;
unsigned int yThres = 127 + 30;
unsigned int zThres = 153 + 30;

unsigned int xLED = 0; // 0 = off, 1 = on
unsigned int yLED = 0;
unsigned int zLED = 0;

unsigned int xLEDcounter = 0;
unsigned int yLEDcounter = 0;
unsigned int zLEDcounter = 0;

#define PJ_ALLON (BIT0 + BIT1 + BIT2 + BIT3) // bits 0-3

/**
 * main.c
 */
int main(void)
{
    WDTCTL = WDTPW | WDTHOLD;   // stop watchdog timer

    // Set P2.7 output to high to power ADC
    P2DIR |= BIT7;
    P2OUT |= BIT7;

    // Clock configuration
    CSCTL0 = 0xA500;                    // Write password to modify CS registers
    CSCTL1 = DCOFSEL0 + DCOFSEL1;           // DCO = 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = DCO, ACLK = DCO, SMCLK = DCO
    CSCTL3 = DIVA_5;                    // Set divider to 1/32

    // Timer A configuration
    TA0CTL = MC_1 + TASSEL_1;           //Select ACLK running at 250kHz
    TA0CCTL0 = CCIE;                    //Enable CCR0 interrupt
    TA0CCR0 = 10000;                   //Set CCR0 at 10000 to create 25 Hz pulse

    // ADC Configuration
    ADC10CTL0 &= ~ADC10ENC;             //Disable ADC
    ADC10CTL0 |= ADC10SHT_2 + ADC10ON;  //Set for 16 clk cycles, turn ADC On
    ADC10CTL1 |= ADC10SHP + ADC10CONSEQ_0; //Sourced from sampling timer, single conversion
    ADC10CTL2 |= ADC10RES;              //Set to 10 bit resolution
    ADC10MCTL0 = ADC10INCH_12;  // ADC input ch A12
    ADC10IE |= ADC10IE0;              //Enable interrupts on conversion complete

    // Configure internal reference
    while (REFCTL0 & REFGENBUSY)
        ;              // If ref generator busy, WAIT
    REFCTL0 |= REFVSEL_0 + REFON;               // Select internal ref = 1.5V

    // Configure UCA0 for 9600 baud
    UCA0CTLW0 = UCSSEL_3;
    UCA0BRW = 52;
    UCA0MCTLW = 0x4900 + UCOS16 + UCBRF0;

    // Configure P2.0 & P2.1 for UCA0
    P2SEL0 &= ~BIT0 + ~BIT1;
    P2SEL1 |= BIT0 + BIT1;

    _EINT();            //Enable Global Interrupts

    state = 1;

    PJDIR |= PJ_ALLON;                        // Set to output
    PJOUT &= ~PJ_ALLON;                        // Set initial LED

    while (1)
    {
        _NOP();
    }

    return 0;
}

void sendXData()
{
    //Output bytes
    UCA0TXBUF = 255;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = 0x01;                     //Send xAccel value
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = xThres;                     //Send zAccel value
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = xResult;                     //Send yAccel value
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
}

void sendYData()
{
    //Output bytes
    UCA0TXBUF = 255;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = 0x02;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = yThres;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = yResult;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
}

void sendZData()
{
    //Output bytes
    UCA0TXBUF = 255;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = 0x03;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = zThres;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
    UCA0TXBUF = zResult;
    while ((UCA0IFG & UCTXIFG) == 0)
        ;
}

#pragma vector = ADC10_VECTOR
__interrupt void ADC10_ISR(void)
{
    if (ADC10IV == 12)
    {
        //Disable ADC so I can change sample ports
        ADC10CTL0 &= ~ADC10ENC;
        if (state == 1)
        {
            //Read xAccel data, set up for yAccel next
            xResult = ADC10MEM0 >> 2;
            state = 2;
            ADC10MCTL0 = ADC10INCH_13; // A13 = Y
        }
        else if (state == 2)
        { //Read yAccel data, set up for zAccel next
            yResult = ADC10MEM0 >> 2;
            state = 3;
            ADC10MCTL0 = ADC10INCH_14; // A14 = Z
        }
        else if (state == 3)
        { //Read zAccel data, set up for xAccel next
            zResult = ADC10MEM0 >> 2;
            state = 1;
            ADC10MCTL0 = ADC10INCH_12; // A12 = X
        }
    }
}

#pragma vector = TIMER0_A0_VECTOR
__interrupt void TIMER0_A0_ISR(void)
{
    int i;
    //Sample x, y, and z using the ADC
    for (i = 0; i < 3; i++)
    {
        ADC10CTL0 |= ADC10ENC + ADC10SC;           //Start sample and conversion
        while ((ADC10CTL1 & ADC10BUSY) == 0)
            ;         //Wait for conversion complete
    }

    // quiz stuff

    if ((xResult > xThres) && xLED == 0)
    {
        sendXData();
        PJOUT |= BIT0;
        xLED = 1;
    }

    if ((yResult > yThres) && (yLED == 0))
    {
        sendYData();
        PJOUT |= BIT1;
        yLED = 1;
    }

    if ((zResult > zThres) && (zLED == 0))
    {
        sendZData();
        PJOUT |= BIT2;
        zLED = 1;
    }

    if (xLED == 1)
    {
        xLEDcounter = xLEDcounter + 1;
        if (xLEDcounter > 51)
        { // 25Hz, 50 ticks, 2 secs
            xLED = 0;
            xLEDcounter = 0;
            PJOUT &= ~BIT0;
        }
    }

    if (yLED == 1)
    {
        yLEDcounter = yLEDcounter + 1;
        if (yLEDcounter > 51)
        { // 25Hz, 50 ticks, 2 secs
            yLED = 0;
            yLEDcounter = 0;
            PJOUT &= ~BIT1;
        }
    }

    if (zLED == 1)
    {
        zLEDcounter = zLEDcounter + 1;
        if (zLEDcounter > 51)
        { // 25Hz, 50 ticks, 2 secs
            zLED = 0;
            zLEDcounter = 0;
            PJOUT &= ~BIT2;
        }
    }
}
