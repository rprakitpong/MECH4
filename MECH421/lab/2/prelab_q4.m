c = tf([14540 3200*14540], [1 0]);
p = tf([11*.2/1000], [.001 3.2]);
l = c*p;
h = .2/1000;
g = l/h;
r = -1/5000;
rs = .2;
tfunc = -rs*(g/(1+g*h))*r;
options = bodeoptions;
options.FreqUnits = 'Hz'; % or 'rad/second', 'rpm', etc.
bode(tfunc);
disp(bandwidth(tfunc));
%[Gm,Pm,Wcg,Wcp] = margin(tfunc);
%step(tfunc);
%stepinfo(tfunc);