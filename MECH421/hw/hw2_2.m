A = tf([10000000], [1 0]);
r1 = 1000;
r2 = 9000;
rm = 4.8;
Lm = .001;
rs = .2;
Kt = .25;
Jm = 1;
Jw = 9;
%Jm = 1*0.0001; % use SI
%Jw = 9*0.0001;
J = Jm + Jw;
Zeq = tf([Kt*Kt*J 0], [J*Lm J*rm Kt*Kt]);
I_V = (Zeq+rs)*(A/(1+A*(r2/(r1+r2))));
bode(I_V);