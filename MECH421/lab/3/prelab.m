%{
J1 = 0.000190;
J2 = 0.000204;
J3 = 0.000166;
m1 = 0.4455;
m2 = 4.2882;
r = 0.0316;
Jsum = J1 + J2 + J3 + r*r*(m1+m2);
disp(Jsum);
syms s;
a = 10*(0.000175388/(1+s/8042))*(-0.1963)*(1/(0.0053*s*s))*0.035*0.1;
disp(a);
sys = tf([-44456773269050453311/195535487181321247129600], [1/8042 1 0 0]);
sys;
h = bodeplot(sys);
setoptions(h,'FreqUnits','Hz','Xlim',[0.1,10000],'MagUnits','abs','MagScale','log');
%}
load('Lab3-Plant-FRF-2021');
freq = FRF(1,:);
P_mes = FRF(2,:);
Mag_mes = abs(P_mes);
Ang_mes = unwrap(angle(P_mes))*180/pi;
%subplot(2,1,1);
%loglog(freq(:,1:160), Mag_mes(:,1:160));
%subplot(2,1,2);
%semilogx(freq(:,1:160), Ang_mes(:,1:160));
w = 100;
phi = 60; % deg
phi = phi * pi/180;
a = (1+sin(phi))/(1-sin(phi));
t = 1/(sqrt(a)*w);
K = 1000; % found that with K=1, mag at w is 1
Ki = w/10;
G = tf([1 Ki], [1 0]);
C = tf([K*a*t K], [t 1]);
C = C*G;
C_frf = [];
for i = freq(1,:)
    j = evalfr(C,i);
    C_frf = [C_frf j];
end
res = [];
for k = 1:size(C_frf,2)
    temp = C_frf(k)*Mag_mes(1,k);
    res = [res temp];
end
% P(s)C(s)
loglog(freq(:,1:160), res(:,1:160));
%
loglog(freq(:,1:160), Ang_mes(:,1:160));