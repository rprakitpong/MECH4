clc
clear all
close all

%% Problem 1
m1 = 1/99;
m2 = 1;
k = 100;

s = tf('s');
H11 = (m2*s^2+k)/(m1*m2*s^4 + (m1+m2)*k*s^2);
H21 = k/(m1*m2*s^4 + (m1+m2)*k*s^2);

h = figure(1)
set(h, 'Position', [0 0 1000 400]+100)
subplot(121)
pzmap(H11)
axis equal
subplot(122)
bode(H11)
grid on 

h = figure(2)
set(h, 'Position', [0 0 1000 400]+100)
subplot(121)
pzmap(H21)
axis equal
subplot(122)
bode(H21)
grid on 

h = figure(3)
set(h, 'Position', [0 0 1000 400]+100)
subplot(121)
pzmap(H11)
hold on
pzmap(H21)
axis equal
legend('H_11','H_{21}')
subplot(122)
bode(H11)
hold on
bode(H21)
grid on 
legend('H_11','H_{21}')

%% Problem 2
clear all
close all

s = tf('s');

Ga = 10/(s^2+101*s+100);
Gb = 10/(s^2+20*s+100);
Gc = 10/(s^2+2*s + 100);
Gd = s/(s^2+2*s + 100);

h = figure(1);
set(h, 'Position', [0 0 1000 400]+100)
subplot(121)
pzmap(Ga)
xlim([-100 100])
axis equal
subplot(122)
bode(Ga)
grid on 

h = figure(2);
set(h, 'Position', [0 0 1000 400]+100)
subplot(121)
pzmap(Gb)
xlim([-100 100])
axis equal
subplot(122)
bode(Gb)
grid on 

h = figure(3);
set(h, 'Position', [0 0 1000 400]+100)
subplot(121)
pzmap(Gc)
xlim([-10 10])
axis equal
subplot(122)
bode(Gc)
grid on 

h = figure(4);
set(h, 'Position', [0 0 1000 400]+100)
subplot(121)
pzmap(Gd)
xlim([-10 10])
axis equal
subplot(122)
bode(Gd)
grid on 
% figure(1)
% bode(Ga)
% hold on
% bode(Gb)
% bode(Gc)
% bode(Gd)
% grid on

wn = 10
zeta = 0.1
wd = wn*sqrt(1-zeta^2)
sigma = wn*zeta

Q = 1/(2*zeta)
wr = wn*sqrt(1-2*zeta^2)
Mr = 1/(2*zeta*sqrt(1-zeta^2))


%%


t = 0:0.001:100;

zeta = 0.1;
wn = 1;
sigma = wn*zeta;
wd = wn*sqrt(1-zeta^2);

x1 = 1-exp(-sigma*t).*(cos(wd*t)+sigma/wd*sin(wd*t));

M = sqrt(1 + sigma^2/wd^2);
phi = atan(sigma/wd);
x2 = 1-exp(-sigma*t).*M.*cos(wd*t-phi);
et = 1+M*exp(-sigma*t);
eb = 1-M*exp(-sigma*t);
figure(1)
plot(t,x1)
grid on
hold on
% plot(t,x2)
plot(t,et)
plot(t,eb)