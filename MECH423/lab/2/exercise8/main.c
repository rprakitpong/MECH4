#include "msp430.h"

volatile unsigned int ADCResult = 0;
volatile unsigned int currLED = 0; // current highest on LED

void Setup(void)
{
    // Set clock ~8MHz
    CSCTL0_H = 0xA5;                            // Unlock register
    CSCTL1 |= DCOFSEL0 + DCOFSEL1;              // Set max. DCO setting
    CSCTL2 = SELA_1 + SELS_3 + SELM_3;          // set ACLK = vlo; MCLK = DCO
    CSCTL3 = DIVA_0 + DIVS_0 + DIVM_0;          // set all dividers
    CSCTL0_H = 0x01;                            // Lock Register

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
    UCA0CTL1 = UCSSEL_2;                        // Set SMCLK as UCLk
    UCA0BR0 = 52;                               // look at the tables
    UCA0BR1 = 0;
    UCA0MCTLW = 0x4911;
    UCA0CTL1 &= ~UCSWRST;                       // release from reset

    // set up thermistor
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
    ADC10CTL0 = ADC10SHT_7 + ADC10ON;           // ADC10ON, S&H=192 ADC clks
    ADC10CTL1 = ADC10SHS_0 + ADC10SHP + ADC10SSEL_0;
    ADC10CTL2 = ADC10RES;                       // 10-bit conversion results
    ADC10MCTL0 = ADC10INCH_4;                   // A4 ADC input select; Vref=AVCC
    ADC10IE = ADC10IE0;                         // Enable ADC conv complete interrupt
}

unsigned int GetRoomTempValue(void)
{
    // get average of the first 50 points to use as datum for LED lights going up and down
    unsigned char CalibCounter = 0;
    unsigned int Value = 0;

    while (CalibCounter < 50)
    {
        CalibCounter++;
        while (ADC10CTL1 & BUSY);
        ADC10CTL0 |= ADC10ENC | ADC10SC;        // Start conversion
        __bis_SR_register(CPUOFF + GIE);        // LPM0, ADC10_ISR will force exit
        __no_operation();
        Value += ADCResult;
    }
    Value = Value / 50;
    return Value;
}

void LEDSequence(unsigned int DiffValue)
{
    int init = 0;                              // initial hurdle that reading has to be above for 2nd LED to light up, reduce noise
    int inc = 1;                                // increment per each new LED to light up
    int LED[8] = { BIT0, BIT1, BIT2, BIT3, BIT4, BIT5, BIT6, BIT7 };
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

void main(void)
{
    // this code is based on some sample code i found online
    WDTCTL = WDTPW + WDTHOLD;                               // Stop WDT
    Setup();

    unsigned int RoomTempValue = GetRoomTempValue();        // get room temp value of thermistor reading

    while (1)
    {
        __delay_cycles(316000);                             // experimentally determined delay to get 25Hz out

        while (ADC10CTL1 & BUSY);
        ADC10CTL0 |= ADC10ENC | ADC10SC;                    // Take ADC measurement
        __bis_SR_register(CPUOFF + GIE);
        __no_operation();

        // LED stuff
        if (ADCResult <= RoomTempValue)                     // doesn't do anything with LED if it gets colder
        {
            LEDSequence(RoomTempValue - ADCResult);         // send over the difference
        }

        while (!(UCA0IFG & UCTXIFG));                       // wait for buffer to be ready
        UCA0TXBUF = ADCResult;                              // write to UART
    }
}

#pragma vector=ADC10_VECTOR
__interrupt void ADC10_ISR(void)
{
    ADCResult = ADC10MEM0 >> 2;
    __bic_SR_register_on_exit(CPUOFF);
}

