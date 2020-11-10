Ke = 10/3.1415;
Ka = .887;
Kt = .72;
Je = .0007;
Be = .00612;
T = .0002;

H = tf((Ke*Ka*Kt),[Je Be 0]);
Hd = c2d(H,T);
step(Hd);
saveas(gcf, 'q2-function.jpg');

A = [-T*Be/Je+1, 0; Ke, 1];
B = [(Ka*Kt/Je)*(T-(Be/Je)*(T*T/2)), (-1/Je)*(T-(Be/Je)*(T*T/2)); (Ka*Kt/Je)*(Ke*T*T/2), -T/Je];
C = [1, 0; 0, 1];
D = [0, 0; 0, 0];
sys = ss(A, B, C, D, T);
figure(2);
step(sys);
saveas(gcf, 'q2-ss.jpg');