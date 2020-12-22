% MECH 467
% Tutorial 8
% Nov. 9, 2020

clear
close all
clc

%% Q1

s=tf('s');
G=28900/(s*(s^2+240*s+28900));
[mag,phase,wout] = bode(G,0.1:10:10000);
mag=squeeze(mag);
phase=squeeze(phase);

figure
subplot(2,1,1)
semilogx(wout,20*log10(mag),'b-','LineWidth',2)
set(gca,'FontName','Euclid','Fontsize',12)
xlabel('Frequency (rad/s)','FontSize',20,'FontWeight','bold','interpreter','latex')
ylabel('Magnitude (dB)','FontSize',20,'FontWeight','bold','interpreter','latex')
title('Bode Diagram','FontSize',20,'interpreter','latex')
grid on


subplot(2,1,2)
semilogx(wout,phase,'b-','LineWidth',2)
set(gca,'FontName','Euclid','Fontsize',12)
xlabel('Frequency (rad/s)','FontSize',20,'FontWeight','bold','interpreter','latex')
ylabel('Phase (deg.)','FontSize',20,'FontWeight','bold','interpreter','latex')
grid on


%% Q2

figure
margin(G); 
[Gm,Pm,Wg,Wp]=margin(G);

% Gain Margin - Hand Calculation
wp_h = 170;
K_wp = 28900/sqrt((wp_h^2*240)^2+(28900*wp_h-wp_h^3)^2);
Gm_h=1/K_wp;

% Phase Margin - Hand Calculation
syms wg_h
wg_h=solve((240*(wg_h^2))^2+(-wg_h^3+28900*wg_h)^2==28900^2);
wg_h=double(wg_h);
wg_h = wg_h(real(wg_h)>0 & imag(wg_h)==0)
Pm_h = 180+(-90-atan2((240*wg_h),(28900*wg_h-wg_h^3))*180/pi)


%% Q3

wc = 60;
K_wc = 28900/sqrt((wc^2*240)^2+(28900*wc-wc^3)^2);
Kp_wc60 = 1/K_wc

figure
margin(G); 
hold on
margin(Kp_wc60*G)
legend('G','G_wc60')

%% Q4

Kp_1=100;
Kp_2=500;
figure
nyquist(Kp_1*G); 
hold on; 
ylim([-3 3])
nyquist(Kp_2*G); 
ylim([-3 3]);
legend('Kp=100','Kp=500')

figure
rlocus(G,(0:10:1000))