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
;  MSP430FR57x Demo - MPU Write protection violation - PUC
;
;  Description: The MPU segment boundaries are defined by:
;  Border 1 = 0xC800 [segment #: 4]
;  Border 2 = 0xD000 [segment #: 8]
;  Segment 1 = 0xC200 - 0xC7FF
;  Segment 2 = 0xC800 - 0xCFFF
;  Segment 3 = 0xD000 - 0xFFFF
;  Segment 2 is write protected. Any write to an address in the segment 2 range
;  causes a PUC. The LED toggles due to repeated PUCs. A delay is included
;  so the debugger can gain access via JTAG.
;
;  ACLK = n/a, MCLK = SMCLK = TACLK = default DCO = ~625 KHz
;
;           MSP430FR5739
;         ---------------
;     /|\|               |
;      | |               |
;      --|RST            |
;        |               |
;        |           P1.0|-->LED
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
ptr     .set    R5
data    .set    R6
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
            bis.b   #BIT0,&P1DIR            ; Configure P1.0 for LED
            bit.b   #MPUSEG2IFG,&MPUCTL1
            jz      Delay
            xor.b   #BIT0,&P1OUT            ; Toggle LED
Delay       mov.w   #15000,R15
L1          dec.w   R15
            jnz     L1
SetupMPU    mov.w   #MPUPW,&MPUCTL0         ;Write PWD to acces MPU registers
            mov.w   #0x0804,&MPUSEG         ; B1 = 0xC800; B2 = 0xD000
                                            ; Borders are assigned to segments
            bic.w   #MPUSEG2WE,&MPUSAM      ; Segment 2 is protected from write
            bis.w   #MPUSEG2VS,&MPUSAM      ; Violation select on write access
            mov.w   #MPUPW+MPUENA,&MPUCTL0 ; Enable MPU protection
                                            
            mov.w   #0x88,data
            mov.w   #0xC802,ptr
            mov.w   data,0(ptr)

Mainloop    jmp     Mainloop                ; Code never gets here
            nop
            
;------------------------------------------------------------------------------
;           Interrupt Vectors
;------------------------------------------------------------------------------
            .sect   ".reset"                ; MSP430 RESET Vector
            .short  RESET                   ;
            .end
 
