%% Nyquist Examples
% Author:   Minkyun Noh
% Date:     2021/03/31

clc
clear all
close all

% Default setting
set(0,'defaultAxesFontSize','default');
set(0,'defaultTextFontSize','default');
set(groot,'defaulttextinterpreter','default');
set(groot,'defaultAxesTickLabelInterpreter','default')
set(groot,'defaultLegendInterpreter','default');
set(0,'defaultlinelinewidth','default');


theta = 0:0.01:2*pi;
uCirc = exp(theta*i);
s = tf('s');

%% First-order system 

Kp = 5;
P = 100/(s+100);

L = Kp*P;

figure(1)
margin(L)
grid on

figure(2)
nyquist(L)
axis equal
hold on
plot(real(uCirc),imag(uCirc),'r:','linewidth',1)
hold off

%% First-order system with delay

Kp = 5;
T = 0e-3;
P = 100/(s+100)*exp(-s*T);

% Loo return ratio
L = Kp*P;

figure(1)
margin(L)
grid on

figure(2)
nyquist(L)
axis equal


%% Second-order system with delay 

T = 5e-3;  % Time delay
zeta = 0.1;
wn = 100;
k = 1;

s = tf('s');

P = wn^2/(s^2 + 2*zeta*wn*s + wn^2)*exp(-s*T);


figure(1)
margin(P)
grid on

figure(2)
nyquist(P)
axis equal


