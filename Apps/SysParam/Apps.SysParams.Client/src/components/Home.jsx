import React from 'react';
import SystemParametersStore from '../stores/SystemParametersStore.jsx';
import {Link} from 'react-router';
import AppDispatcher from '../dispatcher/AppDispatcher.jsx';

export default class Main extends React.Component {

  constructor() {
    super();
    this.state = {items: []};
  }

  componentDidMount() {
    SystemParametersStore.addChangeListener(() => {
      this.setState({items: SystemParametersStore.getItems()});
    });
    SystemParametersStore.loadAll();
  }

  onDeleteClickedFactory(code) {
    return (evt) => {
      evt.preventDefault();

      AppDispatcher.dispatchRemoveSystemParameter(code);
    }
  }

  render() {

    var emptyContent;
    if(!this.state.items || this.state.items.length ==0) {
      emptyContent = (<tr><td colSpan="8" style={{'textAlign': 'center'}}>Brak danych</td></tr>);
    }

    return (
      <div>
        <table className="table table-hover">
          <tbody>
            <tr>
              <th>Akcja</th>
              <th>Kod</th>
              <th>Typ</th>
              <th>Typ wartosci</th>
              <th>Wartosc</th>
              <th>Utworzone przez</th>
              <th>Data utworzenia</th>
              <th>Ostatnio zmodyfikowane przez</th>
              <th>Data ostatniej modyfikacji</th>
            </tr>
            {emptyContent}
            {this.state.items.map((item) => {
              return (
                <tr>
                  <td>
                    <a href="#" onClick={this.onDeleteClickedFactory(item.Code).bind(this)}>
                      <i className="fa fa-remove">

                      </i>
                    </a>
                  </td>
                  <td>{item.Code}</td>
                  <td>{item.ParameterType}</td>
                  <td>{item.ValueType}</td>
                  <td>{item.SerializedValue}</td>
                  <td>{item.CreatedBy}</td>
                  <td>{item.CreatedOn}</td>
                  <td>{item.LastUpdatedBy}</td>
                  <td>{item.LastUpdatedOn}</td>
                </tr>
            );
            })}
          </tbody>
        </table>
        <hr/>
        <Link to="add">Dodaj parametr systemowy</Link>
      </div>
    );
  }
}
