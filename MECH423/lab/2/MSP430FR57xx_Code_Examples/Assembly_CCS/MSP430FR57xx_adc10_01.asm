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
;*******************************************************************************
;   MSP430FR57xx Demo - ADC10, Sample A1, AVcc Ref, Set P1.0 if A1 > 0.5*AVcc
;
;   Description: A single sample is made on A0 with reference to AVcc.
;   Software sets ADC10SC to start sample and conversion - ADC10SC
;   automatically cleared at EOC. ADC10 internal oscillator times sample (16x)
;   and conversion. In Mainloop MSP430 waits in LPM0 to save power until ADC10
;   conversion complete, ADC10_ISR will force exit from LPM0 in Mainloop on
;   reti. If A1 > 0.5*AVcc, P1.0 set, else reset.
;
;                MSP430F5739
;             -----------------
;         /|\|              XIN|-
;          | |                 |
;          --|RST          XOUT|-
;            |                 |
;        >---|P1.1/A1      P1.0|--> LED
;
;  T. Witt
;  Texas Instruments Inc.
;  September 2011
;  Built with Code Composer Studio V4.2
;*******************************************************************************
 .cdecls C,LIST,  "msp430.h"
;-------------------------------------------------------------------------------
            .def    RESET                   ; Export program entry-point to
                                            ; make it known to linker.
ADC_Result  .set    R5
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
StopWDT     mov.w   #WDTPW+WDTHOLD,&WDTCTL      ; Stop WDT
SetupP1     bis.b   #001h,&P1DIR                ; P1.0 output
SetupADC10  bis.b   #002h,&P1SEL0               ;
            bis.b   #002h,&P1SEL1               ;
            mov.w   #ADC10SHT_2+ADC10ON,&ADC10CTL0 ; 16x
            bis.w   #ADC10SHP,&ADC10CTL1        ; ADCCLK = MODOSC; sampling timer
            bis.w   #ADC10RES,&ADC10CTL2        ; 10-bit conversion results
            bis.b   #ADC10INCH_1,&ADC10MCTL0    ; A1 ADC input select; Vref=AVCC
            bis.w   #ADC10IE0,&ADC10IE          ; Enable ADC conv complete interrupt
                                                ;
Mainloop    mov.w   #2500,R15                   ; Delay ~5000 cycles between conversions
L1          dec.w   R15                         ; Decrement R15
            jnz     L1                          ; Delay over?
            bis.w   #ADC10ENC+ADC10SC,&ADC10CTL0 ; Start sampling/conversion
            nop
            bis.w   #CPUOFF+GIE,SR              ; Enter LPM0 with interrupt
            nop                                 ; for debug only
            bic.b   #01h,&P1OUT                 ; P1.0 = 0
            cmp.w   #01FFh,ADC_Result           ; ADCMEM = A1 > 0.5*Vcc?
            jlo     Mainloop                    ; Again
            bis.b   #01h,&P1OUT                 ; P1.0 = 1
            jmp     Mainloop                    ; Again
            nop

;-------------------------------------------------------------------------------
ADC10_ISR;  ADC10 interrupt service routine
;-------------------------------------------------------------------------------
            add.w   &ADC10IV,PC                 ; add offset to PC
            reti                                ; No Interrupt
            reti                                ; Conversion result overflow
            reti                                ; Conversion time overflow
            reti                                ; ADHI
            reti                                ; ADLO
            reti                                ; ADIN
            mov.w   &ADC10MEM0,ADC_Result       ; ADC10IFG0
            bic.w   #CPUOFF,0(SP)               ; Exit LPM0 on reti
            reti

;------------------------------------------------------------------------------
;           Interrupt Vectors
;------------------------------------------------------------------------------
            .sect   ".reset"                    ; MSP430 RESET Vector
            .short  RESET                       ;
            .sect   ADC10_VECTOR                ; ADC10 Vector
            .short  ADC10_ISR                   ;
            .end
