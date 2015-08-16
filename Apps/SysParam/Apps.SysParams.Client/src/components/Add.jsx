import React from 'react';
import SingleValueEditor from './SingleValueEditor.jsx';
import ListValueEditor from './ListValueEditor.jsx';
import AppDispatcher from '../dispatcher/AppDispatcher.jsx';
import { Navigation } from 'react-router';

export default class Add extends React.Component {

  constructor() {
    super();

    this.state = {
      code: '',
      isCodeValid: true,
      description: '',
      paramType: 'Static',
      valueType: 'Text',
      singleValue: '',
      listValue: []
    };
  }

  handleCodeChange(event) {
    if(!event.target.value) {
        this.setState({
          code: event.target.value,
          isCodeValid: false
        });
    } else {
      this.setState({
        code: event.target.value,
        isCodeValid: true
      });
    }
  }

  handleDescriptionChange(event) {
    this.setState({description: event.target.value});
  }

  handleParamTypeChange(event) {
    this.setState({paramType: event.target.value});
  }

  handleValueTypeChange(event) {
    this.setState({valueType: event.target.value});
  }

  handleSingleValueChange(value) {
    this.setState({
      singleValue: value
    });
  }

  handleListValueChange(value) {
    this.setState({
      listValue: value
    });
  }

  validateState(state) {
    var valid = this.refs.valueControl.validate() && this.state.code;
    if(!this.state.code) {
      this.setState({
        isCodeValid: false
      });
    } else {
      this.setState({
        isCodeValid: true
      });
    }

    return valid;
  }

  save(event) {

    event.preventDefault();

    var state = this.state;
    if(this.validateState(state)) {
      AppDispatcher.dispatchAddSystemParameter({
        code: state.code,
        description: state.description,
        paramType: state.paramType,
        valueType: state.valueType,
        serializedValue: () => {
          if(this.state.paramType === 'Static')
            return JSON.stringify(state.singleValue);
          if(this.state.paramType === 'List')
            return JSON.stringify(state.listValue);
        }
      });
      Navigation.transitionTo('Home');
    }
  }

  render() {

    Navigation.context = {
      router: this.context.router
    };

    var valueControl = "";
    if(this.state.paramType == 'Static') {
      valueControl = (<SingleValueEditor ref="valueControl" value={this.state.singleValue} valueType={this.state.valueType} showLabel={true} onChange={this.handleSingleValueChange.bind(this)} />);
    } else if(this.state.paramType == 'List') {
      valueControl = (<ListValueEditor ref="valueControl" value={this.state.listValue} valueType={this.state.valueType} onChange={this.handleListValueChange.bind(this)} />);
    }

    var codeValidationClass = "form-group";
    if(!this.state.isCodeValid) {
      codeValidationClass += " has-error";
    }

    return (
      <div className="row">
        <div className="col-md-6 col-md-offset-3">
        <form role="form">
          <div className={codeValidationClass}>
            <label>Kod</label>
            <input type="text" ref="code" value={this.state.code} className="form-control" placeholder="Kod parametru systemowego" onChange={this.handleCodeChange.bind(this)}/>
          </div>
          <div className="form-group">
            <label>Opis</label>
            <textarea className="form-control" value={this.state.description} ref="description" rows="3" placeholder="Opis parametru systemowego" onChange={this.handleDescriptionChange.bind(this)}></textarea>
          </div>
          <div className="form-group">
            <label>Typ parametru</label>
            <select className="form-control" ref="paramType" value={this.state.paramType} onChange={this.handleParamTypeChange.bind(this)} >
              <option value="Static">Pojedyncza wartosc</option>
              <option value="List">Lista elementow</option>
              <option value="Dictionary">Słownik</option>
            </select>
          </div>
          <div className="form-group">
            <label>Typ wartosci</label>
            <select className="form-control" ref="valueType" value={this.state.valueType} onChange={this.handleValueTypeChange.bind(this)}>
              <option value="Text">Tekst</option>
              <option value="Int">Liczba całkowita</option>
              <option value="Float">Liczba rzeczywista</option>
              <option value="Date">Data</option>
              <option value="DateTime">Data i czas</option>
              <option value="Boolean">Wartosc prawda / fałsz</option>
            </select>
          </div>
          {valueControl}
          <div>
            <button className="btn btn-default pull-right" onClick={this.save.bind(this)}>Zapisz</button>
          </div>
        </form>
        </div>
      </div>

    );
  }
}

Add.contextTypes = {
    router: React.PropTypes.object
};
