import StoreBase from './StoreBase.jsx';
import SystemParameterActions from '../actions/SystemParametersActions.jsx';

class SystemParametersStore extends StoreBase {

    constructor() {
      super({
        'SYS_PARAM_ADD': (data) => {
          this.localAdd(data);
        },

        'SYS_PARAM_UPDATE': (data) => {

        },

        'SYS_PARAM_DELETE': (data) => {
          this.localRemove(data);
        }
      })
    }

    loadAll() {

    }

    loadOne(id) {

    }

}

let instance = new SystemParametersStore();

export default instance;
