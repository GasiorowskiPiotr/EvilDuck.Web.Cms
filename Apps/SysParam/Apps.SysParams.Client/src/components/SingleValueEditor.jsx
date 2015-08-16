import React from 'react';

export default class SingleValueEditor extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      value: props.value || "",
      valueType: props.valueType || 'Text',
      isValueValid: true
    };
  }

  wrapEditor(editor) {
    var label = this.props.showLabel ? (<label>Wartosc</label>) : "";
    var validitiClass = "form-group";
    if(!this.state.isValueValid) {
      validitiClass += " has-error";
    }
    return (
      <div className={validitiClass}>
        {label}
        {editor}
      </div>
    );
  }

  validate() {
    var valid = this.state.isValueValid && this.state.value && this.valueType !== 'Boolean';
    if(!valid) {
      this.setState({
        isValueValid: false
      });
    } else {
      this.setState({
        isValueValid: true
      });
    }
    return valid;
  }

  componentWillReceiveProps(newProps) {
    this.setState({
      value: newProps.value,
      valueType: newProps.valueType
    });
  }

  handleValueChange(event) {
    if(event.target.type == 'checkbox') {
      this.setState ({
        value: event.target.checked
      });

      this.props.onChange(event.target.checked);
    } else {
      if(event.target.value) {
        this.setState({
          value: event.target.value,
          isValueValid: true
        });
      } else {
        this.setState({
          value: event.target.value,
          isValueValid: false
        });
      }


      this.props.onChange(event.target.value);
    }
  }

  render() {

    var editor;
    if(this.state.valueType === 'Text') {
      editor = (<input type="text" className="form-control" value={this.state.value} onChange={this.handleValueChange.bind(this)} />);
    } else if(this.state.valueType === 'Int') {
      editor = (<input type="number" step="1" className="form-control" value={this.state.value} onChange={this.handleValueChange.bind(this)} />);
    } else if(this.state.valueType === 'Float') {
      editor = (<input type="number" step="0.01" className="form-control" value={this.state.value} onChange={this.handleValueChange.bind(this)} />);
    } else if(this.state.valueType === 'Date') {
      editor = (<input type="date" className="form-control" value={this.state.value} onChange={this.handleValueChange.bind(this)} />);
    } else if(this.state.valueType === 'DateTime') {
      editor = (<input type="datetime-local" className="form-control" value={this.state.value} onChange={this.handleValueChange.bind(this)} />);
    } else if(this.state.valueType === 'Boolean') {
      editor = (<input type="checkbox" checked={this.state.value} onChange={this.handleValueChange.bind(this)} />);
    }

    return this.wrapEditor(editor);
  }

}
