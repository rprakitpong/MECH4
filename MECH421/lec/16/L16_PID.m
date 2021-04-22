%% PID controller design for a free mass
% Author:   Minkyun Noh
% Date:     2021/03/11

set(0,'defaultAxesFontSize',12);
set(0,'defaultTextFontSize',12);
set(groot,'defaulttextinterpreter','latex');
set(groot,'defaultAxesTickLabelInterpreter','default')
set(groot,'defaultLegendInterpreter','latex');
set(0,'defaultlinelinewidth',1);

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
title('Loop return ratio')

% Closed-loop transfer functions
Gxd = P/(1+L);
Gxr = L/(1+L);
Gur = C/(1+L);
Ger = 1/(1+L);

figure(2)
subplot(221)
bodemag(Gxr)
title('Complementary Sensitivity')
xlim([1 10^4])
grid on

subplot(222)
bodemag(Ger)
title('Sensitivity')
xlim([1 10^4])
grid on

subplot(223)
bodemag(Gxd)
title('Load Sensitivity')
xlim([1 10^4])
grid on

subplot(224)
bodemag(Gur)
title('Noise Sensitivity')
xlim([1 10^4])
grid on
    

figure(3)
step(Gxr)
grid on
title('Reference to output')
xlim([0 0.3])
     
figure(4)
step(Gxd)
grid on
title('Disturbance to output')
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
title('Loop return ratio')

% Closed-loop transfer functions
Gxd = P/(1+L);
Gxr = L/(1+L);
Gur = C/(1+L);
Ger = 1/(1+L);

figure(2)
subplot(221)
bodemag(Gxr)
title('Complementary Sensitivity')
xlim([1 10^4])
grid on

subplot(222)
bodemag(Ger)
title('Sensitivity')
xlim([1 10^4])
grid on

subplot(223)
bodemag(Gxd)
title('Load Sensitivity')
xlim([1 10^4])
grid on

subplot(224)
bodemag(Gur)
title('Noise Sensitivity')
xlim([1 10^4])
grid on

figure(3)
step(Gxr)
grid on
title('Reference to Output')
xlim([0 0.3])
     
figure(4)
step(Gxd)
grid on
title('Disturbance to Output')
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
Gur = C/(1+L);
Ger = 1/(1+L);

figure(2)
subplot(221)
bodemag(Gxr)
title('Complementary Sensitivity')
xlim([1 10^4])
grid on

subplot(222)
bodemag(Ger)
title('Sensitivity')
xlim([1 10^4])
grid on

subplot(223)
bodemag(Gxd)
title('Load Sensitivity')
xlim([1 10^4])
grid on

subplot(224)
bodemag(Gur)
title('Noise Sensitivity')
xlim([1 10^4])
grid on

figure(3)
step(Gxr)
grid on
title('Reference to Output')
xlim([0 0.3])
     
figure(4)
step(Gxd)
grid on
title('Disturbance to Output')
xlim([0 0.3])
    