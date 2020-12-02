name = 'lbw.mat';
data = load(name);
time_ = data.output.Data(:,1);
x_ref_ = data.output.Data(:,2);
y_ref_ = data.output.Data(:,4);
x_sim_ = data.output.Data(:,3);
y_sim_ = data.output.Data(:,5);

toPrint = [];
flag = true;
i = 1;
while flag
    time = time_(i, 1);
    x_ref = x_ref_(i, 1);
    y_ref = y_ref_(i, 1);
    x_sim = x_sim_(i, 1);
    y_sim = y_sim_(i, 1);
    if x_ref >= 40 && y_ref >= 30
        flag = false;
    else
        if time >= 0.8615
            e_x = x_ref - x_sim;
            e_y = y_ref - y_sim;
            e_c = sqrt(e_x*e_x + e_y*e_y);
            toPrint = [toPrint; time, x_ref, y_ref, x_sim, y_sim, e_x, e_y, e_c];
        end
        i = i + 1;
    end
end

%disp(toPrint);
disp(max(toPrint(:,8)));