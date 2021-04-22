%% DT Approximation of CT Systems
% Author:   Minkyun Noh
% Date:     2020/03/13

clc
clear all
close all

%% Stability Comparison: Forward vs. Backward vs. Tustin

syms s z T

% CT Transfer Function (sym)
wn = 10;
zeta = 0.3;
Hc_sym(s) = wn^2/(s^2+2*wn*zeta*s+wn^2);    

% Hc_sym(s) = s/(s+10)^2;

% DT Transfer Function (sym)
T = 0.1;                   
forward  = (z-1)/T;         % Forward Rectangular (Euler)
backward = (z-1)/z/T;       % Backward Rectangular
tustin   = (z-1)/(z+1)*2/T; % Tustin

Hd_sym(z) = subs(Hc_sym(s),s,backward);      

% CT/DT Transfer Functions (char)
Hc_char = char(Hc_sym);
Hd_char = char(Hd_sym);

% CT/DT Transfer Functions (tf)
s = tf('s');
z = tf('z',T);
eval(['Hc = ',Hc_char]);
eval(['Hd = ',Hd_char]);

figure(1)
    subplot(121)
    pzmap(Hc)
    axis equal
    subplot(122)
    pzmap(Hd)
    axis equal
    
figure(2)
    step(Hc)
    hold on 
    step(Hd)
    grid on
    xlim([0 4])
    ylim([0 2])
    legend('CT system','DT approximation')
    hold off
    
%% Conversion using built-in MATLAB command: c2d

Hd2 = c2d(Hc,T,'tustin')
figure(3)
    step(Hc)
    hold on 
    step(Hd2)
    grid on
    xlim([0 4])
    ylim([0 2])
    legend('CT system','DT approximation')
    hold off

