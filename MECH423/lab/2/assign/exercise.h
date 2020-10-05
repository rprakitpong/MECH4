/*
 * exercise.h
 *
 *  Created on: Sep. 30, 2020
 *      Author: Frienddo
 */

#ifndef EXERCISE_H_
#define EXERCISE_H_


#define LED1 BIT0
#define LED2 BIT1
#define LED3 BIT2
#define LED4 BIT3
#define LED5 BIT4
#define LED6 BIT5
#define LED7 BIT6
#define LED8 BIT7

#define PJ_INIT (LED1 + LED2)   // 0011
#define PJ_ALLON (LED1 + LED2 + LED3 + LED4) // bits 0-3
#define PJ_ALLOFF 0
#define P3_INIT (LED5 + LED8)   // 1001
#define P3_ALLON (LED5 + LED6 + LED7 + LED8) // bits 4-7
#define P3_ALLOFF 0

extern int exercise1(void);
extern int exercise2(void);
extern int exercise3(void);
extern int exercise4(void);


#endif /* EXERCISE_H_ */
