%% PID controller design for a free mass
% Author:   Minkyun Noh
% Date:     2021/03/11

clc
clear all
close all
%% Plant 
s = tf('s');
m = 1;
P = 1/(m*s^2);

%% Lead copmpensation

wc = 100;  % target crossover
alpha = 10;
tau = 1/(wc*sqrt(alpha));
Lead = (alpha*tau*s+1)/(tau*s+1);

Kp = 1/abs(squeeze(freqresp(P,wc)))/sqrt(alpha);
C = Kp*Lead;

% Loop return ratio
L = C*P;
figure(1)
    bode(L) 
    grid on
    xlim([1 10^4])

% Closed-loop transfer functions
Gxd = P/(1+L);
Gxr = L/(1+L);
Gur = C/(1+L);

figure(2)
    subplot(121)
    bode(Gxr)
    xlim([1 10^4])
    grid on
    title('R to X')
    subplot(122)
    step(Gxr)
    grid on
    title('R to X')
    xlim([0 0.3])
     
figure(3)
    subplot(121)
    bode(Gxd)
    xlim([1 10^4])
    grid on
    title('D to X')
    subplot(122)
    step(Gxd)
    grid on
    title('D to X')
    xlim([0 0.3])
    
%% PI compensation
wi = wc/10;
PI = 1+wi/s;
C = Kp*PI*Lead;

% Loop transfer function
L = C*P;
figure(1)
    bode(L) 
    grid on
    xlim([1 10^4])

% Closed-loop transfer functions
Gxd = P/(1+L);
Gxr = L/(1+L);

figure(2)
    subplot(121)
    bode(Gxr)
    xlim([1 10^4])
    grid on
    title('R to Y')
    subplot(122)
    step(Gxr)
    grid on
    title('R to Y')
    xlim([0 0.3])
    
figure(3)
    subplot(121)
    bode(Gxd)
    xlim([1 10^4])
    grid on
    title('D to X')
    subplot(122)
    step(Gxd)
    grid on
    title('D to X')
    xlim([0 0.3])


%% Low-pass filter
wf = 10*wc;
LPF = 1/(s/wf+1);
C = Kp*PI*Lead*LPF;

% Loop transfer function
L = C*P;
figure(1)
    bode(L) 
    grid on
    xlim([1 10^4])

% Closed-loop transfer functions
Gxd = P/(1+L);
Gxr = L/(1+L);

figure(2)
    subplot(121)
    bode(Gxd)
    xlim([1 10^4])
    grid on
    title('D to X')
    subplot(122)
    step(Gxd)
    grid on
    title('Y/D')
    xlim([0 0.3])
    
figure(3)
    subplot(121)
    bode(Gxr)
    xlim([1 10^4])
    grid on
    title('R to X')
    subplot(122)
    step(Gxr)
    grid on
    title('Y/R')
    xlim([0 0.3])