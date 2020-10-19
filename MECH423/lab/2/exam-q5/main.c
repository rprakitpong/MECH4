#include <msp430.h> 

/**
 * main.c
 *
 *
 * 5. Computer Controlled Glowing Orb
Set up TB1.1 to output a PWM signal to P3.4.
Write code to slowly vary the brightness of the LED back and forth to create a glowing orb effect. Hint: vary the PWM duty cycle in the infinite loop and use a delay loop with _NOP() statements to generate a delay time to hold each brightness state. Remember to declare your variables as ‘volatile’.
Set up the UART to operate at 9600 baud, 8, N, 1, to accept incoming data.
Use a circular buffer to temporarily store the following message packet:
Start byte

Command byte

Data byte

0xFF

0x01, 0x02, or 0x04

0x00 to 0xFE

Create a glowing orb with the following behavior:
If Command byte == 0x01: the orb brightness changes from dark to bright, and then repeats
If Command byte == 0x02: the orb brightness changes from bright to dark, and then repeats
If Command byte == 0x04: the orb goes dark
The data byte controls the oscillation period. Note: You may need to scale the data byte to make the oscillation period appreciable.
Record a video of your glowing orb and upload to Canvas.
 */

#define LEN 51
#define TRUE 1
#define FALSE 0

#define PJ_ALLON (BIT0 + BIT1 + BIT2 + BIT3) // bits 0-3
#define PJ_ALLOFF 0

volatile unsigned int read = 0;
volatile unsigned char readByte = 0;
volatile unsigned int write = 0;
volatile unsigned char buffer[51] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0 };
volatile unsigned int yeet = 0;

unsigned int plus1mod51(unsigned int i)
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

int readBuf()
{
    //read from buffer
    if (read == write)
    {
        return FALSE;
    }
    else
    {
        readByte = buffer[read];
        // doesn't delete old data from buffer, just wait for it to be overwritten later
        read = plus1mod51(read);
        return TRUE;
    }
}

int writeBuf(char n)
{
    if (read == plus1mod51(write))
    {
        // is full
        return FALSE;
    }
    else
    {
        buffer[write] = n;
        write = plus1mod51(write);
        return TRUE;
    }
}

int getBufCount()
{
    if (write >= read)
    {
        return write - read;
    }
    else
    {
        // read < write == write at beginning of array
        return (LEN - read) + write; // i think this is correct lol
    }
}

int main(void)
{
    WDTCTL = WDTPW | WDTHOLD;   // stop watchdog timer

    // Configure clocks
    CSCTL0 = 0xA500;                    // Write password to modify CS registers
    CSCTL1 = DCOFSEL0 + DCOFSEL1;           // DCO = 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = DCO, ACLK = DCO, SMCLK = DCO

    // Configure ports for UCA0
    P2SEL0 &= ~(BIT0 + BIT1);
    P2SEL1 |= BIT0 + BIT1;

    // Configure UCA0
    UCA0CTLW0 = UCSSEL0;
    UCA0BRW = 52;
    UCA0MCTLW = 0x4900 + UCOS16 + UCBRF0;
    UCA0IE |= UCRXIE;

    // global interrupt enable
    _EINT();

    // Configure LED out
    PJDIR |= PJ_ALLON;                        // Set to output
    PJOUT &= PJ_ALLOFF;                       // Set initial LED

    // Set Timer B
    P3DIR |= BIT4;
    P3SEL1 &= ~BIT4;
    P3SEL0 |= BIT4;

    // Set Timer B PWMs, one at 100% and one at 25%
    TB1CCR0 = 2000;
    TB1CCTL1 = OUTMOD_7;
    TB1CCR1 = 1000;
    TB1CTL = TBSSEL_2 + MC_1 + TBCLR;

    int state = 3;
    int val = 0;
    int count = 0;
    int pwm = 0;

    while (1)
    {
        if (getBufCount() >= 3)
        {
            readByte = 0;
            while (!(readByte == 0xFF))
            { // find start byte and remove it
                readBuf();
            }
            readBuf();
            state = readByte;
            readBuf();
            val = 10*readByte; // can do 10*(254-readByte) if want high val for low osc period
        }

        if (state == 3)
        {
            TB1CCR1 = 0;
        }
        if (state == 1)
        {
            count++;
            if (count > val)
            {
                TB1CCR1 = pwm + 100;
                if (pwm + 100 >= 2000)
                {
                    pwm = 0;
                }
                else
                {
                    pwm = pwm + 100;
                }
                count = 0;
            }

        }

        if (state == 2)
        {
            count++;
            if (count > val)
            {
                TB1CCR1 = pwm - 100;
                if (pwm - 100 <= 0)
                {
                    pwm = 2000;
                }
                else
                {
                    pwm = pwm - 100;
                }
                count = 0;
            }

        }

    }

    return 0;
}

#pragma vector = USCI_A0_VECTOR
__interrupt void USCI_A0_ISR(void)
{
    unsigned char RxByte;
    RxByte = UCA0RXBUF;
//write to buffer
    if (writeBuf(RxByte) == FALSE)
    {
// send 0xFF 3 times as error msg if write fail
        while ((UCA0IFG & UCTXIFG) == 0)
            ;
        UCA0TXBUF = 0xFF;
        while ((UCA0IFG & UCTXIFG) == 0)
            ;
        UCA0TXBUF = 0xFF;
        while ((UCA0IFG & UCTXIFG) == 0)
            ;
        UCA0TXBUF = 0xFF;
    }

}
