import AppDispatcher from '../dispatcher/AppDispatcher.jsx';
import {EventEmitter} from 'events';
import _ from 'lodash' ;

export default class StoreBase extends EventEmitter {

  constructor(actionHandlers) {
    super();
    this._handlers = actionHandlers;
    this._items = [];

    AppDispatcher.register((payload) => {
      var action = payload.action;

      if(this._handlers[action]) {
        this._handlers[action](payload.data);
        this._emitChange();
      }
    });
  }

  localAdd(item) {
    this._items.push(item);
  }

  localDelete(prop, val) {
    _.remove(this._items, obj);
  }

  getItems() {
    return this._items;
  }

  filterItems(predicate, order, take, skip) {
    var res = this._items;
    if(predicate) {
      res = _.filter(res, predicate);
    }

    if(order) {
      res = _.sortBy(res, order);
    }

    if(skip) {
      res = _.drop(res, skip);
    }

    if(take) {
      res = _.take(res, take);
    }

    return res;
  }

  _emitChange() {
    this.emit('change');
  }

  // Add change listener
  addChangeListener(callback) {
    this.on('change', callback);
  }

  // Remove change listener
  removeChangeListener(callback) {
    this.removeListener('change', callback);
  }

  batchLocalLoad(items) {
    for(var item of items) {
      this._items.push(item);
    }

    this._emitChange();
  }
}
