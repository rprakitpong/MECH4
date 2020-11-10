Ke = 10/3.1415;
Ka = .887;
Kt = .72;
Je = .0007;
Be = .00612;
T = .0002;
K = 100;

H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
Hd = c2d(H,T);                  % discrete time
bode(Hd);