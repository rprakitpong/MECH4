%% Lead Compensator
% Author: Minkyun Noh
% Data: 2021-01-07

set(0,'defaultAxesFontSize',15);
set(0,'defaultTextFontSize',15);
set(groot,'defaulttextinterpreter','latex');
set(groot,'defaultAxesTickLabelInterpreter','default')
set(groot,'defaultLegendInterpreter','latex');
set(0,'defaultlinelinewidth',1);

%% 
clc
clear all
close all

%% Proportional control (2nd order)
clc
clear all

% Controller gain
Kp = 10000; 

% System parameters
m = 1;      % [kg]
b = 10;     % [N.s/m]

s = tf('s');
P = 1/(m*s^2 + b*s);
L = Kp*P;
T = L/(1+L);

% Loop vs. Closed-loop
figure(1)
bode(L)
hold on
bode(T,'r')
xlim([10^-2 10^3]);
legend;
hold off
grid on

% Step response - before compensation
figure(2)
step(T,'r')
ylim([0 2])
grid on

%% Lead compensator design
alpha = 10;     
w_max = 100;
tau = 1/(w_max*sqrt(alpha));

Lead = (alpha*tau*s + 1)/(tau*s + 1);

Kp = 10000/sqrt(alpha);     % Kp divided by sqrt(alpha) to set wc = wmax
C = Kp*Lead;
L_new = C*P;
T_new = L_new/(1+L_new);

% Lead compensation: before vs. after
figure(3)
bode(L)
hold on
grid on
bode(L_new)
hold off
xlim([10^-1 10^3]);
legend;

% Step resposne
figure(5)
step(T)
grid on
hold on
step(T_new)
legend;

