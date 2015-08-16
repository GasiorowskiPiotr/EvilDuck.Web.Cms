import React from 'react';
import _ from 'lodash';
import SingleValueEditor from './SingleValueEditor.jsx';

export default class ListValueEditor extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      value: props.value || [],
      valueType: props.valueType || 'Text'
    };
  }

  componentWillReceiveProps(newProps) {
    this.setState({
      value: newProps.value,
      valueType: newProps.valueType
    });
  }

  wrapEditor(editor) {
    return (
      <div className="form-group">
        <span>
          <label>Wartosci</label>
          <button className="btn btn-default pull-right" onClick={this.add.bind(this)}>Dodaj</button>
        </span>
        <hr />
        {editor}
      </div>
    );
  }

  createOnChangeHandler(idx) {
    return (v) => {
      var iidx = idx;
      var arr = this.state.value;
      arr[iidx] = v;
      this.setState({
        value: arr
      });
    };
  }

  createDeleteHandler(idx) {
    return (evt) => {
      evt.preventDefault();

      var arr = this.state.value;
      _.pullAt(arr, idx);
      this.setState({
        value: arr
      });
    }
  }

  add(event) {
    event.preventDefault();

    var arr = this.state.value;
    arr.push("");

    this.setState({
      value: arr
    });
  }

  validate() {
    return _.every(this.state.value, (v) => {
      if(v) return true;
      else return false;
    });
  }

  render() {

    var editor = _.map(this.state.value, (v, idx) => {
      return (
        <div className="row">
          <div className="col-md-9">
            <SingleValueEditor value={v} showLabel={false} valueType={this.state.valueType} onChange={this.createOnChangeHandler(idx).bind(this)}/>
          </div>
          <div className="col-md-2">
            <button className="btn btn-default" onClick={this.createDeleteHandler(idx).bind(this)}>Usuń</button>
          </div>
        </div>);
    });

    return this.wrapEditor(editor);
  }

}
