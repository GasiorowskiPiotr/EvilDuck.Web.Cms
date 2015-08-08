import {Dispatcher} from 'flux';
import SystemParametersActions from '../actions/SystemParametersActions.jsx';

class AppDispatcher extends Dispatcher {

  _dispatchViewAction(action, data) {
    this.dispatch({
      action: action,
      data: data
    });
  }

  dispatchAddSystemParameter(data) {
    this._dispatchViewAction(SystemParametersActions.SYS_PARAM_ADD, data);
  }

  dispatchRemoveSystemParameter(data) {
    this._dispatchViewAction(SystemParametersActions.SYS_PARAM_DELETE, data);
  }


}

let instance = new AppDispatcher();

module.exports = instance;
