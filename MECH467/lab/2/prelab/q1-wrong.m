Ke = 10/3.1415;
Ka = .887;
Kt = .72;
Je = .0007;
Be = .00612;
T = .0002;

H = tf((Ke*Ka*Kt),[Je (.3*Je+Be) .3*Be]);
Hd = c2d(H,T);
[num, dem] = tfdata(Hd, 'v');
disp(num);
disp(dem);

syms z;
A = -Je*Je/(Be*(.3*Je-Be)); 
B = Je/(Be*(.3*Je-Be)); 
C = 1/Be;
f = Ke*Ka*Kt*(1-1/z)*((A/Je)/(1-exp(-T*Be/Je)/z)+B/(1-exp(-.3*T)/z)+(C/.3)*(1-exp(-.3*T))*(1/z)/((1-1/z)*(1-exp(-.3*T)/z)));
s = simplify(f);
disp(s);