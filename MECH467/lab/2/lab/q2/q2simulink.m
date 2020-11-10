% p controller 
pController
scopeConfig = get_param('pController/Scope','ScopeConfiguration');
scopeConfig.DataLogging = true;
scopeConfig.DataLoggingSaveFormat = 'Dataset';
out = sim('pController');
printSSE('p controller', out);

% LL controller
llController
scopeConfig = get_param('llController/Scope','ScopeConfiguration');
scopeConfig.DataLogging = true;
scopeConfig.DataLoggingSaveFormat = 'Dataset';
out = sim('llController');
printSSE('ll controller', out);

% LLI controller
lliController
scopeConfig = get_param('lliController/Scope','ScopeConfiguration');
scopeConfig.DataLogging = true;
scopeConfig.DataLoggingSaveFormat = 'Dataset';
out = sim('lliController');
printSSE('lli controller', out);


function printSSE(name, out)
    ramp = out.ScopeData{1}.Values.Data(:,1);
    response = out.ScopeData{1}.Values.Data(:,2);
    sse = abs(response(25001, 1) - ramp(25001, 1)); % use last value, guaranteed steady state
    disp(name);
    disp(sse);
end