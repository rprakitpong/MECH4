#include <msp430.h>


volatile unsigned int xResult;
volatile unsigned int yResult;
volatile unsigned int zResult;
volatile unsigned int state = 1;
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
    CSCTL0 = 0xA500;                        // Write password to modify CS registers
    CSCTL1 = DCOFSEL0 + DCOFSEL1;           // DCO = 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = DCO, ACLK = DCO, SMCLK = DCO
    CSCTL3 = DIVA_5;                    // Set divider to 1/32

    // Timer A configuration
    TA0CTL = MC_1 + TASSEL_1;           //Select ACLK running at 250kHz
    TA0CCTL0 = CCIE;                    //Enable CCR0 interrupt
    TA0CCR0 = 10000;                    //Set CCR0 at 10000 to create 25 Hz pulse

    // ADC Configuration
    ADC10CTL0 &= ~ADC10ENC;             //Disable ADC
    ADC10CTL0 |= ADC10SHT_2 + ADC10ON;  //Set for 16 clk cycles, turn ADC On
    ADC10CTL1 |= ADC10SHP + ADC10CONSEQ_0;              //Sourced from sampling timer, single conversion
    ADC10CTL2 |= ADC10RES;              //Set to 10 bit resolution
    ADC10MCTL0 = ADC10INCH_12;  // ADC input ch A12
    ADC10IE |= ADC10IE0;                //Enable interrupts on conversion complete

    // Configure internal reference
    while(REFCTL0 & REFGENBUSY);              // If ref generator busy, WAIT
    REFCTL0 |= REFVSEL_0+REFON;               // Select internal ref = 1.5V

    // Configure UCA0 for 9600 baud
    UCA0CTLW0 = UCSSEL_3;
    UCA0BRW = 52;
    UCA0MCTLW = 0x4900 + UCOS16 + UCBRF0;

    // Configure P2.0 & P2.1 for UCA0
    P2SEL0 &= ~BIT0 + ~BIT1;
    P2SEL1 |= BIT0 + BIT1;

    _EINT();            //Enable Global Interrupts

    state = 1;

    while(1)
    {
        _NOP();
    }

    return 0;
}

#pragma vector = ADC10_VECTOR
__interrupt void ADC10_ISR (void)
{
    if(ADC10IV == 12)
        {
            //Disable ADC so I can change sample ports
            ADC10CTL0 &= ~ADC10ENC;
            if (state==1)
            {
                //Read xAccel data, set up for yAccel next
                xResult = ADC10MEM0 >> 2;
                state = 2;
                ADC10MCTL0 = ADC10INCH_13; // A13 = Y
            }
            else if (state == 2)
            {//Read yAccel data, set up for zAccel next
                yResult = ADC10MEM0 >> 2;
                state = 3;
                ADC10MCTL0 = ADC10INCH_14; // A14 = Z
            }
            else if (state == 3)
            {//Read zAccel data, set up for xAccel next
                zResult = ADC10MEM0 >> 2;
                state = 1;
                ADC10MCTL0 = ADC10INCH_12; // A12 = X
            }
        }
}

#pragma vector = TIMER0_A0_VECTOR
__interrupt void TIMER0_A0_ISR (void)
{
    int i;
    //Sample x, y, and z using the ADC
    for (i = 0; i < 3; i++) {
        ADC10CTL0 |= ADC10ENC + ADC10SC;            //Start sample and conversion
        while((ADC10CTL1 & ADC10BUSY) == 0);         //Wait for conversion complete
    }

    //Output bytes
    UCA0TXBUF = 255;
    while ((UCA0IFG & UCTXIFG)==0);
    UCA0TXBUF = xResult;                     //Send xAccel value
    while ((UCA0IFG & UCTXIFG)==0);
    UCA0TXBUF = yResult;                     //Send yAccel value
    while ((UCA0IFG & UCTXIFG)==0);
    UCA0TXBUF = zResult;                     //Send zAccel value
    while ((UCA0IFG & UCTXIFG)==0);
}
