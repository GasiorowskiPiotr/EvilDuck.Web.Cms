import StoreBase from './StoreBase.jsx';
import SystemParameterActions from '../actions/SystemParametersActions.jsx';
import $ from 'jquery';

class SystemParametersStore extends StoreBase {

    constructor() {
      super({
        'SYS_PARAM_ADD': (data) => {
          return $.ajax({
            method: 'POST',
            url: '../../api/SystemParameters/',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify({
              Code: data.code,
              Description: data.description,
              SerializedValue: data.serializedValue(),
              ParameterType: data.paramType,
              ValueType: data.valueType
            })
          });
        },

        'SYS_PARAM_UPDATE': (data) => {

        },

        'SYS_PARAM_DELETE': (data) => {
          return $.ajax({
            method: 'DELETE',
            url: '../../api/SystemParameters/' + data,
          }).done(() => {
            return this.loadAll();
          })
        }
      })
    }

    loadAll() {
      return $.ajax({
        method: 'GET',
        url: '../../api/SystemParameters',
        accept: 'application/json',
        success: (data) => {
            this.batchLocalLoad(data.Entities);
        }
      });
    }

    loadOne(id) {

    }

}

let instance = new SystemParametersStore();

export default instance;
