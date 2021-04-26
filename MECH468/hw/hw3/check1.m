a=[0 1 0;0 0 1;0 0 -1];
b=[0; 0; 1];
c=[0 1 0;1 1 0];
d=[0; 0];
%{
ctrl=[b a*b a*a*b];
co=colspace(sym(ctrl));
disp(co);
obvs=[c;c*a;c*a*a];
ob=null(sym(obvs));
disp("--");
disp(ob);
%}
t=[1 0 0;0 1 0;0 0 1];
tinv=t;
a_ = t*a*tinv;
b_ = t*b;
c_ = c*tinv;
d_ = d;
check=zpk(ss(a_,b_,c_,d_));
