%% Fractional-order integrator
% Author:   Minkyun Noh
% Date:     2021/01/07

% Tip: ctrlpref

clc
clear all

set(0,'defaultAxesFontSize',12);
set(0,'defaultTextFontSize',12);
set(groot,'defaulttextinterpreter','default');
set(groot,'defaultAxesTickLabelInterpreter','default')
set(groot,'defaultLegendInterpreter','default');
set(0,'defaultlinelinewidth',1);

% Transfer function
s = tf('s');
%% Single integrator & unity feedback
close all

P1 = 1000/s;

figure(1)
hold on
bode(feedback(P1,1),'r')
bode(feedback(P1,10),'r')
bode(feedback(P1,100),'r')
grid on
bode(P1,'b');
xlim([1e2, 1e7])

figure(2)
step(feedback(P1,1),'r')
grid on

% Check rise time (10-90%)
stepinfo(feedback(P1,1))

%% Fractional-order integrator

zeros = 2.^[2:2:30];
poles = 2.^[1:2:29];

P2 = 1/s*zpk(-zeros,-poles,1);

figure(3)
hold on
bode(feedback(P2,1),'r')
bode(feedback(P2,10),'r')
bode(feedback(P2,100),'r')
bode(P2,'b');
grid on
xlim([1e2, 1e7])

figure(4)
step(feedback(P1,1))
hold on 
grid on
step(feedback(P2,1))
xlim([0, 0.016])
xticks([0:0.002:0.012])
legend('Single','Fractional')

% Check rise time (10-90%)
stepinfo(feedback(P2,1))

%% Double integrator & unity feedback
close all

P = 1/s^2;

figure(1)
bode(P,'b');
grid on
hold on
bode(feedback(P,1),'r')
% bode(feedback(P,10),'r')
% bode(feedback(P,100),'r')
% bode(feedback(P,1000),'r')

figure
step(P)
