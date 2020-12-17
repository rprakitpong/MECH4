#include <msp430.h> 

/**
 * main.c
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

volatile unsigned int readEncoder = FALSE;

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
    /*
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
     */
    buffer[write] = n;
    write = plus1mod51(write);
    return TRUE;
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

int parseInt16(char upperByte, char lowerByte, char escByte)
{
    if (escByte == 0x03)
    {
        upperByte = 0xFF;
        lowerByte = 0xFF;
    }
    else if (escByte == 0x02)
    {
        upperByte = 0xFF;
    }
    else if (escByte == 0x01)
    {
        lowerByte = 0xFF;
    }
    int ret = 0;
    ret |= (int) upperByte << 8;
    ret |= (int) lowerByte;
    return ret;
}

volatile int dc_cur_pos = 0;
volatile unsigned int dc_dir = FALSE; // false = +y, true = -y
volatile unsigned int dc_displacement = 0;
volatile int dc_init_pos = 0;
volatile int dc_final_pos = 0;
volatile unsigned int dc_speed = 0;
volatile unsigned int dc_init_speed = 0;
volatile unsigned int encoder0 = 0;
volatile unsigned int encoder1 = 0;

void getDCPosDir()
{
    encoder0 = TA0R;
    encoder1 = TA1R;
    if (dc_dir == TRUE) {
        dc_cur_pos = -encoder1;
    } else {
        dc_cur_pos = encoder0;
    }
    //dc_cur_pos = encoder0 - encoder1;
}

void SetDir(int dir)
{
    if (dir == FALSE)
    {
        // +y
        P3OUT |= BIT5;
        P3OUT &= ~BIT6;
        dc_dir = FALSE;
    }
    else
    {
        // -y
        P3OUT |= BIT6;
        P3OUT &= ~BIT5;
        dc_dir = TRUE;
    }
}

void SetSpeed(int spd)
{
    TB1CCR1 = spd;
    dc_speed = spd;
}

void SetPWM(int displace, int dir, int spd)
{
    if (displace == 0)
    {
        spd = 0;
    }

    SetDir(dir);
    getDCPosDir();
    dc_init_pos = dc_cur_pos;
    dc_displacement = displace;
    if (dc_dir == TRUE)
    {
        // -y
        dc_final_pos = dc_init_pos - dc_displacement;
    }
    else
    {
        dc_final_pos = dc_init_pos + dc_displacement;
    }
    dc_init_speed = spd;
    SetSpeed(spd);
}

void readBufAndSetPWM()
{
    readByte = 0;
    while (!(readByte == 0xFF))
    { // find start byte and remove it
        readBuf();
    }
    int x_stepper = 0; // 0~38, 19 == 0 displace
    int y_dc = 0;
    int y_dir = 0;
    int speed = 0;

    readBuf();
    x_stepper = readByte;
    readBuf();
    y_dc = readByte;
    readBuf();
    speed = readByte * 655;
    readBuf();
    int esc = readByte;

    if (y_dc > 19)
    {
        y_dir = FALSE; // +y
        y_dc = y_dc - 19;
    }
    else
    {
        y_dir = TRUE; // -y
        y_dc = 19 - y_dc;
    }

    SetPWM(y_dc * 40, y_dir, speed);
}

void readEncoderFn()
{
    getDCPosDir();
    int pos = dc_cur_pos;

    int absDiff = pos - dc_final_pos;
    if (pos < dc_final_pos)
    {
        absDiff = dc_final_pos - pos;
    }

    if (absDiff < 1)
    {
        SetSpeed(0);
    }
    else
    {
        if (pos > dc_final_pos)
        {
            SetDir(TRUE);
        }
        else
        {
            SetDir(FALSE);
        }
        SetSpeed(dc_init_speed);
    }

}

int main(void)
{
    WDTCTL = WDTPW | WDTHOLD;   // stop watchdog timer

    // CLOCK
    // Configure clocks
    CSCTL0 = 0xA500;                    // Write password to modify CS registers
    CSCTL1 = DCOFSEL0 + DCOFSEL1;           // DCO = 8 MHz
    CSCTL2 = SELM0 + SELM1 + SELA0 + SELA1 + SELS0 + SELS1; // MCLK = DCO, ACLK = DCO, SMCLK = DCO

    // UART
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

    // PWM
    P3DIR |= BIT5 + BIT6;
    P3OUT |= BIT5;
    P3OUT &= ~BIT6;
    // Set Timer B
    P3DIR |= BIT4;
    P3SEL1 &= ~(BIT4);
    P3SEL0 |= BIT4;
    // Set PWM out
    TB1CCR0 = 65536;
    TB1CCTL1 = OUTMOD_7;
    TB1CCR1 = 0;
    TB1CTL = TBSSEL_2 + MC_2 + TBCLR;

    // READ ENCODER
    // Set Timer A
    P1DIR &= ~(BIT1 + BIT2);
    P1SEL1 &= ~(BIT1 + BIT2);
    P1SEL0 |= (BIT1 + BIT2);
    // Set Timer A for capture
    TA1CTL |= TASSEL_0 + MC_2 + TACLR;
    TA0CTL |= TASSEL_0 + MC_2 + TACLR;

    // INTERRUPT
    TB2CTL |= TBSSEL_1 + MC_1;
    TB2CCR0 = 300;     // arbitrary fast interval
    TB2CCTL0 = CCIE;    // enable interrupt

    while (1)
    {
        if (getBufCount() >= 5)
        {
            readBufAndSetPWM();
        }
        if (readEncoder == TRUE)
        {
            readEncoderFn();
            readEncoder = FALSE;
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

#pragma vector = TIMER2_B0_VECTOR
__interrupt void TIMER2_B0_ISR(void)
{
    readEncoder = TRUE;
}
