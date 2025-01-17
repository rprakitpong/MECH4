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
;  MSP430FR57xx Demo - Software Port Interrupt Service on P1.4 from LPM4
;
;  Description: A Lo/Hi transition on P1.4 will trigger P1ISR the first time. 
;  On hitting the P1ISR, device exits LPM4 mode and executes section of code in
;  main() which includes toggling an LED and the P1.4IES bit which switches between 
;  Lo/Hi and Hi/Lo trigger transitions to alternatively enter the P1ISR. 
;  ACLK = n/a, MCLK = SMCLK = default DCO
;
;               MSP430FR5739
;            -----------------
;        /|\|                 |-
;         | |                 |
;         --|RST              |-
;     /|\   |                 |
;      --o--|P1.4         P1.0|-->LED
;     \|/
;
;   Tyler Witt
;   Texas Instruments Inc.
;   September 2011
;   Built with Code Composer Studio V4.2
;******************************************************************************
 .cdecls C,LIST,  "msp430.h"
;-------------------------------------------------------------------------------
            .def    RESET                   ; Export program entry-point to
                                            ; make it known to linker.

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
StopWDT     mov.w   #WDTPW+WDTHOLD,&WDTCTL  ; Stop WDT
            bis.b   #BIT0,&P1DIR            ; Set P1.0 to output direction
            bic.b   #BIT0,&P1OUT
            bic.b   #BIT4,&P1OUT
            bic.b   #BIT4,&P1IES            ; P1.4 Lo/Hi edge
            mov.b   #BIT4,&P1IE             ; P1.4 interrupt enabled
            clr.b   &P1IFG                  ; P1.4 IFG cleared
Mainloop    nop
            bis.w   #LPM4+GIE,SR            ; Enter LPM4 w/ interrupt
            nop                             ; for debug
            xor.b   #BIT0,&P1OUT            ; P1.0 = toggle
            jmp     Mainloop
            nop                             ; for debug
            
;------------------------------------------------------------------------------
P1_ISR ;    Port 1 Interrupt
;------------------------------------------------------------------------------
            xor.b   #BIT4,&P1IES
            bic.b   #BIT4,&P1IFG            ; Clear P1.4 IFG
            bic.w   #LPM4,0(SP)
            reti
;------------------------------------------------------------------------------
;           Interrupt Vectors
;------------------------------------------------------------------------------
            .sect   ".reset"                ; MSP430 RESET Vector
            .short  RESET                   ;
            .sect   PORT1_VECTOR            ; Port 1 Vector
            .short  P1_ISR                  ;
            .end
 
