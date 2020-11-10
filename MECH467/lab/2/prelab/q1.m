Ke = 10/3.1415;
Ka = .887;
Kt = .72;
Je = .0007;
Be = .00612;
T = .0002;

H = tf((Ke*Ka*Kt),[Je Be 0]);
Hd = c2d(H,T);
[num, dem] = tfdata(Hd, 'v');
disp(num);
disp(dem);

syms z;
f = Ke*Ka*Kt*(1-1/z)*(((-Je/(Be*Be))*(1/z)*(1-exp(-Be*T/Je))/((1-1/z)*(1-exp(-Be*T/Je)/z)))+((1/Be)*T*(1/z)/((1-1/z)*(1-1/z))));
s = simplify(f);
disp(s);