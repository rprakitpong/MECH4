%% MECH 421 - Ball screw stage loop shaping
% Author: Minkyun Noh
% Date: 2021-03-31
% Note: Data from past MECH 421 materials

clc
clear all
close all

% Custom setting
set(0,'defaultAxesFontSize',12);
set(0,'defaultTextFontSize',12);
set(groot,'defaulttextinterpreter','latex');
set(groot,'defaultAxesTickLabelInterpreter','latex')
set(groot,'defaultLegendInterpreter','latex');
set(0,'defaultlinelinewidth',2);


%% Data loading [mm/V] -> [m/V]

% Collocated system
load('RotaryFRF.mat');
    f1=RotaryFrom1To400(:,1);   % [Hz]
    mag1=RotaryFrom1To400(:,2)/1000; % [mm -> m]
    ph1=RotaryFrom1To400(:,3);  % [deg] 
    P1=mag1.*exp(1i*ph1/180*pi);

% Non-collocated system
load('LinearFRF.mat');
    f2=LinearFrom1To400(:,1);   % [Hz]
    mag2=LinearFrom1To400(:,2)/1000; % [mm -> m]
    ph2=LinearFrom1To400(:,3);  % [deg]
    P2=mag2.*exp(1i*ph2/180*pi);

%% Plant Bode plot (measured) P = X/Vr [m/V]

figure(1)
subplot(211)
    loglog(f1,mag1,'r')
    grid on
    hold on
    loglog(f2,mag2,'b')
    xlim([1,400])
    ylim([1e-7 1])
    ylabel('$|P|$ [m/V]')
    legend('Collocated','Non-collocated');
subplot(212);
    semilogx(f1,ph1,'r')
    grid on
    hold on
    semilogx(f2,ph2,'b')    
    xlim([1,400])
    xlabel('Frequency [Hz]')
    yticks([-720:90:0])
    ylabel('$\angle P$ [deg]')

%% Controller design

s = tf('s');

% Lead 
wc = 50*(2*pi);
alpha = 10;
tau = 1/wc/sqrt(alpha);
Lead = (alpha*tau*s + 1)/(tau*s + 1);

% PI 
wi = wc/10;
PI = (1+wi/s);

% Kp
Kp = 1/3e-5/sqrt(alpha);

% Controller
C = Kp*PI*Lead;

% Frequency response
C = squeeze(freqresp(C,f1,'Hz')); 
C_mag = abs(C);
C_phi = angle(C)/pi*180;
    
%% Loop Bode plot

% Collocated System
L1 = C.*P1;
L1_mag = abs(L1);
L1_phi = unwrap(angle(L1))/pi*180;
T1 = L1./(1+L1);
T1_mag = abs(T1);
T1_phi = unwrap(angle(T1))/pi*180;

% Non-collocated System
L2 = C.*P2;
L2_mag = abs(L2);
L2_phi = unwrap(angle(L2))/pi*180;
T2 = L2./(1+L2);
T2_mag = abs(T2);
T2_phi = unwrap(angle(T2))/pi*180;

figure(2)
subplot(211)
    loglog(f1,L1_mag,'r')
    hold on
    grid on
    loglog(f1,L2_mag,'b')
    xlim([1,400])
    ylabel('$|L|$')
    legend('Collocated','Non-collocated');
    hold off
subplot(212)
    semilogx(f1,L1_phi,'r')
    hold on
    grid on
    semilogx(f1,L2_phi,'b')
    xlim([1,400])
    xlabel('Frequency [Hz]')
    ylabel('$\angle L$ [deg]')
    yticks([-720:90:0])
    hold off

%% Nyquist plot

theta = 0:0.01:2*pi;
uCirc = exp(theta*i);

figure(3)
    plot(real(L1),imag(L1),'r')
    hold on
    plot(real(L2),imag(L2),'b')
    plot(-1,0,'k+','HandleVisibility','off')
    plot(real(uCirc),imag(uCirc),'k:','linewidth',1)
    axis equal
    grid on
    xticks([-10:1:10])
    xlim([-4 4])
    yticks([-10:1:10])
    ylim([-4 4])
    legend('Collocated','Non-collocated')
    hold off
    xlabel('Re$\{L\}$')
    ylabel('Im$\{L\}$')