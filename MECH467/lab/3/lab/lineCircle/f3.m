
%{
names = {'f3_200.mat', 'f3_250.mat'};
hold on;
for i = 1:2
    name = names{i};
    data = load(name);
    Tplot = data.txy.t;
    xplot = data.txy.x;
    yplot = data.txy.y;
    plot(Tplot, xplot);
    plot(Tplot, yplot);
end
title('Position over time');
ylabel('Position (mm)');
xlabel('Time (s)');
legend('X Low feedrate','Y Low feedrate','X High feedrate','Y High feedrate');
saveas(gcf, 'F3-additional.png');
%}

figure;
clf();
subplot(2,2,1);
title('R1');
plotter();
legend('Low feedrate', 'High feedrate');
subplot(2,2,2);
title('R2');
plotter();
subplot(2,2,3);
title('R3');
plotter();
subplot(2,2,4);
title('R4');
plotter();

function plotter()
    hold on;

    names = {'f3_200.mat', 'f3_250.mat'};
    for i = 1:2
        % init variables to be loaded into simulink model
        T = 0.0001;
        Ka = 1;
        Kt = 0.49;
        Ke = 1.59;
        Jx = 0.000436;
        Bx = 0.0094;
        Jy = 0.0003;
        By = 0.0091;

        % use LBW LLI controller
        a = 13.9282;
        T_ = 0.0021323;
        Kx = 0.75858;
        Ky = 0.82224;
        Ki = 12.5664;

        LL = tf([a*T_ 1],[T_ 1]);
        I = tf([1 Ki],[1 0]);
        LLI_Lx_z = Kx*c2d(LL*I, T, 'tustin');
        LLI_Ly_z = Ky*c2d(LL*I, T, 'tustin');

        name = names{i};
        data = load(name);
        Tplot = data.txy.t;
        xplot = data.txy.x;
        yplot = data.txy.y;

        sim('e2_sim.slx');

        x = ans.sim.Data(:,3);
        y = ans.sim.Data(:,5);
        plot(x, y);
    end
end

