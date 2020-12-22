%% MECH 467/541
% Tutorial 7
% Nov. 1, 2020

clear
close all
clc
format compact

%% Problem No. 5

zeta=0.7;
wn=0:0.1:100;

figure
for i=1:length(wn)
    pole1=-zeta*wn(i)+wn(i)*sqrt(zeta^2-1);
    pole2=-zeta*wn(i)-wn(i)*sqrt(zeta^2-1);
    
    p1=plot(pole1,'b*'); %works only for zeta<1 so poles are complex
    hold on    
    p2=plot(pole2,'r*');
    hold on
end

hold off
axis equal
set(gca,'FontName','Euclid','Fontsize',12)
xlabel('Real','FontSize',20,'FontWeight','bold','interpreter','latex')
ylabel('Imaginary','FontSize',20,'FontWeight','bold','interpreter','latex')
title(['Varying $\omega_n$ , $\zeta$ = ',num2str(zeta)],'FontSize',20,'interpreter','latex')
legend([p1,p2],{'Pole 1','Pole 2'},'FontSize',20,'FontWeight','bold','location','best','interpreter','latex')
grid on
grid minor
ax=gca;
ax.GridLineStyle='--';
ax.GridAlpha=0.5;
ax.XMinorTick='on';
ax.YMinorTick='on';

%% Problem No. 6

wn=300;
zeta=0:0.001:1;

figure
for i=1:length(zeta)
    pole1=-zeta(i)*wn+wn*sqrt(zeta(i)^2-1);
    pole2=-zeta(i)*wn-wn*sqrt(zeta(i)^2-1);
    
    p1=plot(pole1,'b*');  %works only for zeta<1 so poles are complex
    hold on     
    p2=plot(pole2,'r*');
    hold on
end

hold off
axis equal
set(gca,'FontName','Euclid','Fontsize',12)
xlabel('Real','FontSize',20,'FontWeight','bold','interpreter','latex')
ylabel('Imaginary','FontSize',20,'FontWeight','bold','interpreter','latex')
title(['Varying $\zeta$ , $\omega_n$ = ',num2str(wn)],'FontSize',20,'interpreter','latex')
legend([p1,p2],{'Pole 1','Pole 2'},'FontSize',20,'FontWeight','bold','location','best','interpreter','latex')
grid on
grid minor
ax=gca;
ax.GridLineStyle='--';
ax.GridAlpha=0.5;
ax.XMinorTick='on';
ax.YMinorTick='on';

%% Problem No. 7

Je=7e-4;
Be=0.006;

s=tf('s');
Gol=1/(Je*s^2+Be*s);

figure
rlocus(Gol)