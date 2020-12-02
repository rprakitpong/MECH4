figure;
clf();
subplot(2,2,1);
title('R1');
plotter();
legend('Reference', 'LBW', 'HBW', 'Mixed');
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
    
    name = 'lli.mat';
    lli = load(name);
    time = lli.lli.X.Data;
    x_act = lli.lli.Y(1).Data;
    y_act = lli.lli.Y(2).Data;
    x_ref = lli.lli.Y(3).Data;
    y_ref = lli.lli.Y(4).Data;

    plot(x_ref, y_ref);

    names = {'lbw.mat', 'hbw.mat', 'mix.mat'};
    for i = 1:3
        data = load(names{i});
        x_sim = data.output.Data(:,3);
        y_sim = data.output.Data(:,5);
        plot(x_sim, y_sim);
    end
end