; --COPYRIGHT--,BSD_EX
;  Copyright (c) 2012, Texas Instruments Incorporated
;  All rights reserved.
; 
;  Redistribution and use in source and binary forms, with or without
;  modification, are permitted provided that the following conditions
;  are met:
; 
;  *  Redistributions of source code must retain the above copyright
;     notice, this list of conditions and the following disclaimer.
; 
;  *  Redistributions in binary form must reproduce the above copyright
;     notice, this list of conditions and the following disclaimer in the
;     documentation and/or other materials provided with the distribution.
; 
;  *  Neither the name of Texas Instruments Incorporated nor the names of
;     its contributors may be used to endorse or promote products derived
;     from this software without specific prior written permission.
; 
;  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
;  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
;  THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
;  PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
;  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
;  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
;  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
;  OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
;  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
;  OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
;  EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
; 
; ******************************************************************************
;  
;                        MSP430 CODE EXAMPLE DISCLAIMER
; 
;  MSP430 code examples are self-contained low-level programs that typically
;  demonstrate a single peripheral function or device feature in a highly
;  concise manner. For this the code may rely on the device's power-on default
;  register values and settings such as the clock configuration and care must
;  be taken when combining code from several examples to avoid potential side
;  effects. Also see www.ti.com/grace for a GUI- and www.ti.com/msp430ware
;  for an API functional library-approach to peripheral configuration.
; 
; --/COPYRIGHT--
;******************************************************************************
;  MSP430FR57xx Demo - USCI_B0 I2C Master RX single bytes from MSP430 Slave
;
;  Description: This demo connects two MSP430's via the I2C bus. The master
;  reads from the slave. This is the MASTER CODE. The data from the slave
;  transmitter begins at 0 and increments with each transfer.  
;  The USCI_B0 RX interrupt is used to know when new data has been received.
;  ACLK = n/a, MCLK = SMCLK = BRCLK =  DCO = 1MHz
;
;  *** to be used with "MSP430FR57xx_uscib0_i2c_05.asm" ***
;
;                                /|\  /|\
;               MSP430FR5739      10k  10k     MSP430F5739
;                   slave         |    |        master
;             -----------------   |    |   -----------------
;           -|XIN  P1.6/UCB0SDA|<-|----+->|P1.6/UCB0SDA  XIN|-
;            |                 |  |       |                 | 32kHz
;           -|XOUT             |  |       |             XOUT|-
;            |     P1.7/UCB0SCL|<-+------>|P1.7/UCB0SCL     |
;            |                 |          |             P1.0|--> LED
;
;   Tyler Witt
;   Texas Instruments Inc.
;   August 2011
;   Built with Code Composer Studio V4.2
;******************************************************************************
 .cdecls C,LIST,  "msp430.h"
;-------------------------------------------------------------------------------
            .def    RESET                   ; Export program entry-point to
                                            ; make it known to linker.
RXData      .set    R5
;------------------------------------------------------------------------------
            .global _main
            .global __STACK_END
            .sect   .stack                  ; Make stack linker segment known
            .text                           ; Assemble to Flash memory
            .retain                         ; Ensure current section gets linked
            .retainrefs
;------------------------------------------------------------------------------
_main
RESET       mov.w   #__STACK_END,SP         ; Initialize stackpointer
            mov.w   #WDTPW+WDTHOLD,&WDTCTL  ; Stop WDT
            mov.b   #0xA5,&CSCTL0_H
            bis.w   #DCOFSEL0+DCOFSEL1,&CSCTL1 ; Set max DCO setting
            mov.w   #SELA_3+SELS_3+SELM_3,&CSCTL2 ; ACLK = MCLK = DCO
            mov.w   #DIVA_3+DIVS_3+DIVM_3,&CSCTL3 ; set all dividers to 1MHz
            bis.b   #BIT6+BIT7,&P1SEL1      ; Configure pins
            
            bis.w   #UCSWRST,&UCB0CTLW0     ; Software reset enabled
            bis.w   #UCMODE_3+UCMST+UCSYNC,&UCB0CTLW0 ; I2C mode, Master mode, sync
            bis.w   #UCASTP_2,&UCB0CTLW1    ; automatic stop generated
                                            ; after UCB0TBCNT is reached
            mov.w   #0x0008,&UCB0BRW        ; baudrate = SMCLK / 8
            bis.w   #0x0001,&UCB0TBCNT      ; number of bytes to be received
            mov.w   #0x0048,&UCB0I2CSA      ; slave address
            bic.w   #UCSWRST,&UCB0CTLW0     ; Software reset cleared
            bis.w   #UCRXIE+UCNACKIE+UCBCNTIE,&UCB0IE
            bis.b   #BIT0,&P1DIR

Mainloop    mov.w   #1000,R15               ; Delay to R15
Delay       dec.w   R15
            jnz     Delay
Again       bit.b   #UCTXSTP,&UCB0CTL1      ; Ensure stop condition got sent
            jnz     Again
            bis.b   #UCTXSTT,&UCB0CTL1         
            nop
            bis.w   #LPM0+GIE,SR            ; Enter LPM0 w/ interrupt
            nop
            jmp     Mainloop
            nop                             ; remain in LPM0
            
;------------------------------------------------------------------------------
USCI_ISR ;  USCI Interrupt Service Routine
;------------------------------------------------------------------------------
            add     &UCB0IV,PC              ; Add offset to PC
            reti                            ; No interrupt
            reti                            ; ALIFG break
            jmp     I2CStart                ; NACKIFG break
            reti                            ; STTIFG break
            reti                            ; STPIFG break
            reti                            ; RXIFG3 break
            reti                            ; TXIFG3 break
            reti                            ; RXIFG2 break
            reti                            ; TXIFG2 break
            reti                            ; RXIFG1 break
            reti                            ; TXIFG1 break
            jmp     GetRXData               ; RXIFG0 break
            reti                            ; TXIFG0 break
            jmp     ByteCNT                 ; BCNTIFG break
            reti                            ; clock low timeout break
            reti                            ; 9th bit break
I2CStart    bis.b   #UCTXSTT,&UCB0CTL1      ; I2C start condition
            reti
GetRXData   mov.w   &UCB0RXBUF,RXData       ; Get RX Data
            bic.w   #LPM0,0(SP)             ; Exit LPM0
            reti
ByteCNT     xor.b   #BIT0,&P1OUT
            reti
;------------------------------------------------------------------------------
;           Interrupt Vectors
;------------------------------------------------------------------------------
            .sect   ".reset"                ; MSP430 RESET Vector
            .short  RESET                   ;
            .sect   USCI_B0_VECTOR          ; USCI_B0_VECTOR
            .short  USCI_ISR                ;
            .end
