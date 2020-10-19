/*
 * 4. Inclinometer
Set up timer A to trigger an interrupt every 10 ms (i.e. 100 Hz).
With each timer interrupt, trigger the ADC to sample from the X-axis of the accelerometer.
Set up a circular buffer to store the previous 160 ms of data (i.e. 16 samples) from the X-axis of the accelerometer.
With each new data point, calculate the running average of the previous 16 data points by simply summing the results in a 16-bit variable.
Set up TB1.1 to output a PWM waveform to P3.4 to control the brightness of LED5.
Use the average X-axis acceleration to control the TB1.1 PWM duty cycle, which will control the brightness of LED5. You will have to scale the acceleration result and/or the PWM range to generate an appreciable brightness change between 0 and 90°. Hint: use left and right shifts to multiply and divide by 2.
Record a video of your inclinometer and upload to Canvas.
 */

#include <msp430.h>

volatile unsigned int xResult;

#define LEN 17

volatile unsigned int read = 0;
volatile unsigned int write = 0;
volatile unsigned int buffer[17] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                     0, 0, 0 };
volatile int runningAvg = 0;

unsigned int plus1mod(unsigned int i)
{
    // range: 0~51*2-1
    if (i + 1 < LEN)
    {
        return i + 1;
    }
    else
    {
        return (i + 1) - LEN;
    }
}

int writeData()
{
    //write
    // enable overwrite
    buffer[write] = xResult;
    write = plus1mod(write);
    return 1;

}

int readData()
{
    //read
    if (read == write)
    {
        // empty
        return -1;
    }
    else
    {
        int ret = buffer[read];
        // doesn't delete old data from buffer, just wait for it to be overwritten later
        read = plus1mod(read);
        return ret;
    }
}

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

    // configure P3.4 & P3.5 as output (these are connected to TB1.1 and TB1.2
    P3DIR |= BIT4;
    P3SEL1 &= ~BIT4;
    P3SEL0 |= BIT4;

    // set Timer B in up count mode from SMCLK
    TB1CTL = TBSSEL_2 + MC_1 + TBCLR + TBIE;

    TB1CCR0 = 2000;
    TB1CCTL1 = OUTMOD_7;
    TB1CCR1 = 10;


    // Timer A configuration
    TA0CTL = MC_1 + TASSEL_1;           //Select ACLK running at 250kHz
    TA0CCTL0 = CCIE;                    //Enable CCR0 interrupt
    TA0CCR0 = 2500;                    //Set CCR0 at 2500 to create 100Hz

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

    _EINT();            //Enable Global Interrupts

    while (1)
    {
        _NOP();
        // we know that avg value is 127
        // from lab 1, we know max min about +-35
        // duty vals is 10~1990
        // solving for linear, we get duty=-2592+28x
        int i = -2592 + 28 * runningAvg;
        unsigned int j = i;
        TB1CCR1 = j;
    }

    return 0;
}

#pragma vector = ADC10_VECTOR
__interrupt void ADC10_ISR(void)
{
    if (ADC10IV == 12)
    {
        //Read xAccel data, set up for yAccel next
        xResult = ADC10MEM0 >> 2;
    }
}

#pragma vector = TIMER0_A0_VECTOR
__interrupt void TIMER0_A0_ISR(void)
{

    ADC10CTL0 |= ADC10ENC + ADC10SC;            //Start sample and conversion
    while ((ADC10CTL1 & ADC10BUSY) == 0)
        ;         //Wait for conversion complete
    writeData(); // write to circular buffer

    unsigned int bufferCopy[17] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                    0, 0 }; // make copy of buffer
    int i;
    for (i = 0; i < LEN; i++)
    {
        bufferCopy[i] = buffer[i];
    }
    int readtemp = read;
    int writetemp = write;

    int data = 0;
    int count = -1;
    int sum = 0;
    while (data != -1)
    {
        count = count + 1;
        sum = data + sum;
        data = readData();
    }
    runningAvg = sum / count;

    for (i = 0; i < LEN; i++)
    {
        buffer[i] = bufferCopy[i];
    }

    // copy original back
    read = readtemp;
    write = writetemp;
}
